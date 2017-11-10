using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class PlayerRepository : RepositoryCompetitionId<Player>
    {
        public PlayerRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<Player> GetSortQuery(IQueryable<Player> items) =>
            items.OrderBy(i => i.LastName);

        public async Task SetSelectedStartTime(Player[] players, DateTime startTime)
        {
            Parallel.ForEach(players, p => p.StartTime = startTime);
            await ContextProvider.DoAsync(async ctx =>
            {
                Array.ForEach(players, p => ctx.Entry(p).State = EntityState.Modified);
                await ctx.SaveChangesAsync();
            });
        }

        public async Task<Tuple<Player[], int>> GetAllByFiltersAsync(PlayerFilterOptions filterOptions, int pageNumber, int numberItemsOnPage)
        {
            Player[] players = null;
            int totalItemsNumber = 0;
            await ContextProvider.DoAsync(async ctx =>
            {
                players = await GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                    filterOptions).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage).ToArrayAsync();

                totalItemsNumber = await GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                    filterOptions).CountAsync();
            });

            var totalPageNumber = (int) Math.Ceiling((double) totalItemsNumber / numberItemsOnPage);

            return new Tuple<Player[], int>(players, totalPageNumber);
        }

        private IQueryable<Player> GetFilteredQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions) =>
            GetSortQuery(GetStringQuery(GetWhereQuery(items, filterOptions), filterOptions), filterOptions);

        private IQueryable<Player> GetWhereQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions)
        {
            if (filterOptions.AgeCategory != null)
                items = items.Where(i => i.AgeCategoryId == filterOptions.AgeCategory.Id);

            if (filterOptions.Distance != null)
                items = items.Where(i => i.DistanceId == filterOptions.Distance.Id);

            if (filterOptions.ExtraPlayerInfo != null)
                items = items.Where(i => i.ExtraPlayerInfoId == filterOptions.ExtraPlayerInfo.Id);

            if (filterOptions.Men != null)
                items = items.Where(i => i.IsMale == filterOptions.Men);

            if (filterOptions.WithoutStartTime != null)
                items = items.Where(i => i.IsStartTimeFromReader || i.StartTime == null);

            if (filterOptions.Invalid != null)
                items = items.Where(i => i.AgeCategoryId == null || i.DistanceId == null ||
                                         i.ExtraPlayerInfoId == null);

            return items;
        }

        private IQueryable<Player> GetStringQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions)
        {
            int oneNumenr;
            int twoNumber;

            if (Int32.TryParse(filterOptions.Query, out oneNumenr))
                return items.Where(i => i.StartNumber == oneNumenr);

            string[] numberStrings = filterOptions.Query.Split('-');
            if (numberStrings.Length == 2 && Int32.TryParse(numberStrings[0], out oneNumenr) &&
                Int32.TryParse(numberStrings[2], out twoNumber))
                return items.Where(i => (oneNumenr <= i.StartNumber && i.StartNumber <= twoNumber) || (oneNumenr >= i.StartNumber && i.StartNumber >= twoNumber));

            string[] strings = filterOptions.Query.Split(' ');
            foreach (string s in strings)
                items = items.Where(i => i.FirstName.StartsWith(s) || i.LastName.StartsWith(s) ||
                                         i.Team.StartsWith(s) || i.FullCategory.Contains(s));

            return items;
        }

        private IQueryable<Player> GetSortQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions)
        {
            if (filterOptions.PlayerSort == PlayerSort.ByStartNumber)
                return GetDirectedSortQuery(items, p => p.StartNumber, filterOptions.DescendingSort);

            if (filterOptions.PlayerSort == PlayerSort.ByBirthDate ||
                filterOptions.PlayerSort == PlayerSort.ByStartTime)
                return GetDirectedSortQuery(items, FuncDateTimeFilterSort(filterOptions), filterOptions.DescendingSort);

            return GetDirectedSortQuery(items, FuncStringFilterSort(filterOptions), filterOptions.DescendingSort);
        }

        private IOrderedQueryable<Player> GetDirectedSortQuery<T>(IQueryable<Player> items, Expression<Func<Player, T>> func,
            bool descending) =>
            descending ? items.OrderByDescending(func).ThenBy(p => p.LastName) : items.OrderBy(func).ThenBy(p => p.LastName);

        private Expression<Func<Player, string>> FuncStringFilterSort(PlayerFilterOptions filterOptions)
        {
            switch (filterOptions.PlayerSort)
            {
                case PlayerSort.ByFullCategory:
                    return p => p.FullCategory;
                
                case PlayerSort.ByFirstName:
                    return p => p.FirstName;

                case PlayerSort.ByLastName:
                    return p => p.LastName;

                case PlayerSort.ByTeam:
                    return p => p.Team;

                default:
                    throw new ArgumentException("invalid filterOptions");
            }
        }

        private Expression<Func<Player, DateTime>> FuncDateTimeFilterSort(PlayerFilterOptions filterOptions)
        {
            switch (filterOptions.PlayerSort)
            {
                case PlayerSort.ByBirthDate:
                    return p => p.BirthDate;

                case PlayerSort.ByStartTime:
                    return p => p.StartTime;

                default:
                    throw new ArgumentException("invalid filterOptions");
            }
        }

        public async Task<Player> AddAsync(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo) =>
            await AddAsync(await PreparePlayer(player, distance, extraPlayerInfo));

        public async Task UpdateAsync(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo) =>
            await UpdateAsync(await PreparePlayer(player, distance, extraPlayerInfo));

        public async Task UpdateFullCategoryForAll() =>
            await ContextProvider.DoAsync(async ctx =>
            {
                await ctx.Players.Where(i => i.CompetitionId == Competition.Id).ForEachAsync(i => i.FullCategory =
                    GetFullCategory(i.Distance, i.ExtraPlayerInfo, i.AgeCategory, i.IsMale));
                await ctx.SaveChangesAsync();
            });

        private async Task<Player> PreparePlayer(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo)
        {
            player.DistanceId = distance?.Id;
            player.Distance = null;

            player.ExtraPlayerInfoId = extraPlayerInfo?.Id;
            player.ExtraPlayerInfo = null;

            AgeCategory ageCategory = await (new AgeCategoryRepository(ContextProvider)).GetFitting(player);
            player.AgeCategoryId = ageCategory?.Id;
            player.AgeCategory = null;

            player.FullCategory = GetFullCategory(distance, extraPlayerInfo, ageCategory, player.IsMale);
            return player;
        }

        protected virtual string GetFullCategory(Distance distance, ExtraPlayerInfo extraPlayerInfo, AgeCategory ageCategory,
            bool male) => 
            (distance == null || extraPlayerInfo == null || ageCategory == null) ? null :
            distance.Name.Substring(0, 4) + " " + (male ? "M" : "K") + ageCategory.Name + extraPlayerInfo.ShortName;
    }
} 