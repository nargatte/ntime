using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BaseCore.Csv;
using BaseCore.PlayerFilter;

namespace BaseCore.DataBase
{
    public class PlayerRepository : RepositoryCompetitionId<Player>
    {
        public PlayerRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<Player> GetSortQuery(IQueryable<Player> items) =>
            items.OrderBy(i => i.LastName);

        public async Task SetSelectedStartTimeAsync(Player[] players, DateTime startTime)
        {
            Parallel.ForEach(players, p => p.StartTime = startTime);
            await ContextProvider.DoAsync(async ctx =>
            {
                Array.ForEach(players, p => ctx.Entry(p).State = EntityState.Modified);
                await ctx.SaveChangesAsync();
            });
        }

        public async Task<Tuple<Player[], int>> GetAllByFilterAsync(PlayerFilterOptions filterOptions, int pageNumber, int numberItemsOnPage)
        {
            Player[] players = null;
            int totalItemsNumber = 0;
            await ContextProvider.DoAsync(async ctx =>
            {
                players = await GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                    filterOptions).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage).AsNoTracking().ToArrayAsync();

                totalItemsNumber = await GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                    filterOptions).CountAsync();
            });

            return new Tuple<Player[], int>(players, totalItemsNumber);
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

            if (filterOptions.CompleatedCompetition != null)
                items = items.Where(i => i.CompetitionCompleted == filterOptions.CompleatedCompetition);

            if (filterOptions.HasVoid != null)
                items = items.Where(i => i.TimeReads.All(t => t.TimeReadTypeId != (int) TimeReadTypeEnum.Void) !=
                                         filterOptions.HasVoid);

            return items;
        }

        private IQueryable<Player> GetStringQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions)
        {
            int oneNumenr;
            int twoNumber;

            if (filterOptions.Query == null)
                return items;

            string[] numberStrings = filterOptions.Query.Split('-');
            if (numberStrings.Length == 2 && Int32.TryParse(numberStrings[0], out oneNumenr) &&
                Int32.TryParse(numberStrings[1], out twoNumber))
            {
                if (twoNumber < oneNumenr)
                {
                    int tem = twoNumber;
                    twoNumber = oneNumenr;
                    oneNumenr = tem;
                }
                return items.Where(i => oneNumenr <= i.StartNumber && i.StartNumber <= twoNumber);
            }

            numberStrings = filterOptions.Query.Split(',');
            int[] numers = new int[numberStrings.Length];
            int it = 0;
            while (it < numberStrings.Length)
            {
                if(!Int32.TryParse(numberStrings[it], out numers[it]))
                    break;

                it++;
            }
            if (it == numberStrings.Length)
                return items.Where(i => numers.Contains(i.StartNumber));

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

            if (filterOptions.PlayerSort == PlayerSort.ByStartTime)
                return GetDirectedSortQuery(items,
                    p => (p.StartTime ?? DateTime.MaxValue).Hour * 60 * 60 +
                         (p.StartTime ?? DateTime.MaxValue).Minute * 60 + (p.StartTime ?? DateTime.MaxValue).Second, filterOptions.DescendingSort);

            if (filterOptions.PlayerSort == PlayerSort.ByBirthDate ||
                filterOptions.PlayerSort == PlayerSort.ByStartTime)
                return GetDirectedSortQuery(items, FuncDateTimeFilterSort(filterOptions), filterOptions.DescendingSort);

            if(filterOptions.PlayerSort == PlayerSort.ByRank)
                if (filterOptions.DescendingSort)
                    return items.OrderByDescending(p => p.LapsCount).ThenByDescending(p => p.Time);
                else
                    return items.OrderBy(p => p.LapsCount).ThenBy(p => p.Time);

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
            PrepareToAdd(player);

            player.DistanceId = distance?.Id;
            player.Distance = null;

            player.ExtraPlayerInfoId = extraPlayerInfo?.Id;
            player.ExtraPlayerInfo = null;

            AgeCategory ageCategory = await (new AgeCategoryRepository(ContextProvider, Competition)).GetFitting(player);
            player.AgeCategoryId = ageCategory?.Id;
            player.AgeCategory = null;

            player.FullCategory = GetFullCategory(distance, extraPlayerInfo, ageCategory, player.IsMale);
            return player;
        }

        protected virtual string GetFullCategory(Distance distance, ExtraPlayerInfo extraPlayerInfo, AgeCategory ageCategory,
            bool male) => 
            (distance == null || extraPlayerInfo == null || ageCategory == null) ? null :
            distance.Name.Substring(0, 4) + " " + (male ? "M" : "K") + ageCategory.Name + extraPlayerInfo.ShortName;

        public async Task<Tuple<int, int>> ImportTimeReadsAsync(string fileName, int readerNumber)
        {
            CsvImporter<TimeReadRecord, TimeReadRecordMap> csvImporter = new CsvImporter<TimeReadRecord, TimeReadRecordMap>(fileName);

            TimeReadRecord[] timeReads = await csvImporter.GetAllRecordsAsync();

            HashSet<int> expectedStartNumbers = new HashSet<int>();
            foreach (TimeReadRecord read in timeReads)
                expectedStartNumbers.Add(read.StartNumber);

            Player[] players = null;

            await ContextProvider.DoAsync(async ctx =>
            {
                players = await ctx.Players.Where(p => p.CompetitionId == Competition.Id).Join(expectedStartNumbers, p => p.StartNumber, esn => esn, (p, esn) => p)
                    .AsNoTracking().ToArrayAsync();
            });

            Dictionary<int, Player> dictionaryPlayers = new Dictionary<int, Player>();

            foreach (Player player in players)
                dictionaryPlayers.Add(player.StartNumber, player);

            List<TimeRead> timeReadsList = new List<TimeRead>();

            foreach (TimeReadRecord read in timeReads)
            {
                Player p;
                if(dictionaryPlayers.TryGetValue(read.StartNumber, out p))
                    timeReadsList.Add(new TimeRead(read.Time, readerNumber) {PlayerId = p.Id});
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.TimeReads.AddRange(timeReadsList);
                await ctx.SaveChangesAsync();
            });

            return new Tuple<int, int>(timeReads.Length, timeReadsList.Count);
        }

        public async Task<Tuple<int, int>> ImportPlayersAsync(string fileName)
        {
            CsvImporter<PlayerRecord, PlayerRecordMap> csvImporter = new CsvImporter<PlayerRecord, PlayerRecordMap>(fileName);

            PlayerRecord[] playerRecords = await csvImporter.GetAllRecordsAsync();

            HashSet<string> distanceSet = new HashSet<string>();
            HashSet<string> aditionalInfoSet = new HashSet<string>();

            foreach (PlayerRecord record in playerRecords)
            {
                distanceSet.Add(record.StringDistance);
                aditionalInfoSet.Add(record.StringAditionalInfo);
            }

            Distance[] distances = null;
            ExtraPlayerInfo[] extraPlayerInfos = null;

            await ContextProvider.DoAsync(async ctx =>
            {
                distances = await ctx.Distances.Where(d => d.CompetitionId == Competition.Id).Join(distanceSet, d => d.Name, esn => esn, (d, esn) => d)
                .AsNoTracking().ToArrayAsync();

                extraPlayerInfos = await ctx.ExtraPlayerInfo.Where(e => e.CompetitionId == Competition.Id).Join(aditionalInfoSet, i => i.Name, e => e, (i, e) => i)
                    .AsNoTracking().ToArrayAsync();
            });

            Dictionary<string, Distance> distancesDictionary = new Dictionary<string, Distance>();
            Dictionary<string, ExtraPlayerInfo> extraPlayerInfosDictionary = new Dictionary<string, ExtraPlayerInfo>();

            foreach (Distance distance in distances)
            {
                distancesDictionary.Add(distance.Name, distance);
            }

            foreach (ExtraPlayerInfo extraPlayerInfo in extraPlayerInfos)
            {
                extraPlayerInfosDictionary.Add(extraPlayerInfo.Name, extraPlayerInfo);
            }

            List<Player> players = new List<Player>();

            foreach (PlayerRecord record in playerRecords)
            {
                Player p = new Player(record.FirstName, record.LastName, record.DateBirth, record.IsMale, record.Team,
                    record.StartNumber) {StartTime = record.StartTime};

                Distance distance;
                distancesDictionary.TryGetValue(record.StringDistance, out distance);

                ExtraPlayerInfo extraPlayerInfo;
                extraPlayerInfosDictionary.TryGetValue(record.StringAditionalInfo, out extraPlayerInfo);

                p = await PreparePlayer(p, distance, extraPlayerInfo);

                players.Add(p);
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.Players.AddRange(players);
                await ctx.SaveChangesAsync();
            });

            return new Tuple<int, int>(players.Count, playerRecords.Length);
        }
    }
} 