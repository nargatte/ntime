﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
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

        protected override IQueryable<Player> GetIncludeQuery(IQueryable<Player> items) =>
            items.Include(p => p.Distance).Include(p => p.AgeCategory).Include(p => p.ExtraPlayerInfo);

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
                players = await GetIncludeQuery(GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                    filterOptions).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage)).AsNoTracking().ToArrayAsync();

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
                items = items.Where(i => (i.IsStartTimeFromReader || i.StartTime == null) ^ !filterOptions.WithoutStartTime.Value);

            if (filterOptions.Invalid != null)
                items = items.Where(i => (i.AgeCategoryId == null || i.DistanceId == null ||
                                         i.ExtraPlayerInfoId == null) ^ !filterOptions.Invalid.Value);

            if (filterOptions.CompleatedCompetition != null)
                items = items.Where(i => i.CompetitionCompleted == filterOptions.CompleatedCompetition);

            if (filterOptions.HasVoid != null)
                items = items.Where(i => i.TimeReads.All(t => t.TimeReadTypeId != (int)TimeReadTypeEnum.Void) !=
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
                if (!Int32.TryParse(numberStrings[it], out numers[it]))
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

            if (filterOptions.PlayerSort == PlayerSort.ByRank)
                if (filterOptions.DescendingSort)
                    return items.OrderBy(p => p.LapsCount).ThenByDescending(p => p.Time);
                else
                    return items.OrderByDescending(p => p.LapsCount).ThenBy(p => p.Time);

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

        public async Task<Player> AddAsync(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo)
        {
            player.Distance = distance;
            player.ExtraPlayerInfo = extraPlayerInfo;
            AgeCategory ageCategory = await (new AgeCategoryRepository(ContextProvider, Competition)).GetFittingAsync(player);
            player.AgeCategory = ageCategory;
            await AddAsync(PreparePlayer(player, distance, extraPlayerInfo, ageCategory));
            player.Distance = distance;
            player.ExtraPlayerInfo = extraPlayerInfo;
            player.Distance = distance;
            player.ExtraPlayerInfo = extraPlayerInfo;
            player.AgeCategory = ageCategory;
            return player;
        }

        public async Task UpdateAsync(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo)
        {
            player.Distance = distance;
            player.ExtraPlayerInfo = extraPlayerInfo;
            AgeCategory ageCategory = await (new AgeCategoryRepository(ContextProvider, Competition)).GetFittingAsync(player);
            player.AgeCategory = ageCategory;
            await UpdateAsync(PreparePlayer(player, distance, extraPlayerInfo, ageCategory));
            player.Distance = distance;
            player.ExtraPlayerInfo = extraPlayerInfo;
            player.Distance = distance;
            player.ExtraPlayerInfo = extraPlayerInfo;
            player.AgeCategory = ageCategory;
        }

        public async Task UpdateFullCategoryAllAsync()
        {
            Player[] players = await GetAllAsync();
            AgeCategory[] categories = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                categories = await ctx.AgeCategories.Where(a => a.CompetitionId == Competition.Id)
                    .AsNoTracking().ToArrayAsync();
            });

            Dictionary<int, AgeCategory> ageCategoriesDictionary = new Dictionary<int, AgeCategory>();

            foreach (Player player in players)
            {
                if (!ageCategoriesDictionary.TryGetValue(player.BirthDate.Year, out AgeCategory ageCategory))
                {
                    ageCategory = categories
                        .FirstOrDefault(a =>
                            a.YearFrom <= player.BirthDate.Year && player.BirthDate.Year <= a.YearTo);
                    if (ageCategory != null)
                    {
                        ageCategoriesDictionary.Add(player.BirthDate.Year, ageCategory);
                    }
                }

                player.AgeCategory = ageCategory;
                player.AgeCategoryId = ageCategory?.Id;

                player.FullCategory = GetFullCategory(player.Distance, player.ExtraPlayerInfo, player.AgeCategory,
                    player.IsMale);

                player.AgeCategory = null;
                player.Distance = null;
                player.ExtraPlayerInfo = null;
            }

            await UpdateRangeAsync(players);

        }

        public async Task UpdateRankingAllAsync() =>
            await ContextProvider.DoAsync(async ctx =>
            {
                AgeCategory[] ageCategories = await ctx.AgeCategories.Where(a => a.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                ExtraPlayerInfo[] playerInfos = await ctx.ExtraPlayerInfo.Where(a => a.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                Distance[] distances = await ctx.Distances.Where(a => a.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();

                PlayerFilterOptions filterOptions = new PlayerFilterOptions()
                {
                    PlayerSort = PlayerSort.ByRank,
                    WithoutStartTime = false,
                    Invalid = false,
                    HasVoid = false,
                    CompleatedCompetition = true,
                    DescendingSort = false
                };

                for (int ism = 0; ism <= 1; ism++)
                {
                    filterOptions.Men = ism != 0;
                    foreach (Distance distance in distances)
                    {
                        filterOptions.Distance = distance;
                        filterOptions.AgeCategory = null;
                        filterOptions.ExtraPlayerInfo = null;
                        OrderSet(GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                            filterOptions), (i, player) => player.DistancePlaceNumber = i + 1);
                        foreach (ExtraPlayerInfo extraPlayerInfo in playerInfos)
                        {
                            foreach (AgeCategory ageCategory in ageCategories)
                            {
                                filterOptions.AgeCategory = ageCategory;
                                filterOptions.ExtraPlayerInfo = extraPlayerInfo;
                                OrderSet(GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                                    filterOptions), (i, player) => player.CategoryPlaceNumber = i + 1);
                            }
                        }
                    }
                }

                await ctx.SaveChangesAsync();
            });

        private void OrderSet(IQueryable<Player> players, Action<int, Player> action)
        {
            int i = 0;
            Player[] playersTab = players.ToArray();
            foreach (Player player in playersTab)
            {
                action(i, player);
                i++;
            }
        }

        private async Task<Player> PreparePlayer(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo)
        {
            PrepareToAdd(player);

            player.DistanceId = distance?.Id;
            player.Distance = null;

            player.ExtraPlayerInfoId = extraPlayerInfo?.Id;
            player.ExtraPlayerInfo = null;

            AgeCategory ageCategory = await (new AgeCategoryRepository(ContextProvider, Competition)).GetFittingAsync(player);
            player.AgeCategoryId = ageCategory?.Id;
            player.AgeCategory = null;

            player.FullCategory = GetFullCategory(distance, extraPlayerInfo, ageCategory, player.IsMale);
            return player;
        }

        private Player PreparePlayer(Player player, Distance distance, ExtraPlayerInfo extraPlayerInfo, AgeCategory ageCategory)
        {
            PrepareToAdd(player);

            player.DistanceId = distance?.Id;
            player.Distance = null;

            player.ExtraPlayerInfoId = extraPlayerInfo?.Id;
            player.ExtraPlayerInfo = null;

            player.AgeCategoryId = ageCategory?.Id;
            player.AgeCategory = null;

            player.FullCategory = GetFullCategory(distance, extraPlayerInfo, ageCategory, player.IsMale);
            return player;
        }

        protected virtual string GetFullCategory(Distance distance, ExtraPlayerInfo extraPlayerInfo, AgeCategory ageCategory,
            bool male) =>
            (distance == null || extraPlayerInfo == null || ageCategory == null) ? null :
            distance.Name.Substring(0, 4) + " " + (male ? "M" : "K") + ageCategory.Name + extraPlayerInfo.ShortName;

        public async Task<Tuple<int, int>> ImportTimeReadsAsync(string fileName, Gate gate)
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
                if (dictionaryPlayers.TryGetValue(read.StartNumber, out p))
                    timeReadsList.Add(new TimeRead(read.Time) { PlayerId = p.Id, GateId = gate.Id });
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.TimeReads.AddRange(timeReadsList);
                await ctx.SaveChangesAsync();
            });

            //await UpdateRankingAllAsync();

            return new Tuple<int, int>(timeReads.Length, timeReadsList.Count);
        }

        public async Task<Tuple<int, int>> ImportPlayersAsync(string fileName)
        {
            CsvImporter<PlayerRecord, PlayerRecordMap> csvImporter = new CsvImporter<PlayerRecord, PlayerRecordMap>(fileName);

            PlayerRecord[] playerRecords = await csvImporter.GetAllRecordsAsync();

            Distance[] distances = null;
            ExtraPlayerInfo[] extraPlayerInfos = null;
            AgeCategory[] ageCategories = null;

            await ContextProvider.DoAsync(async ctx =>
            {
                distances = await ctx.Distances.Where(d => d.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                extraPlayerInfos = await ctx.ExtraPlayerInfo.Where(e => e.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                ageCategories = await ctx.AgeCategories.Where(e => e.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
            });

            Dictionary<string, Distance> distancesDictionary = new Dictionary<string, Distance>();
            Dictionary<string, ExtraPlayerInfo> extraPlayerInfosDictionary = new Dictionary<string, ExtraPlayerInfo>();
            Dictionary<int, AgeCategory> ageCategoriesDictionary = new Dictionary<int, AgeCategory>();

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
                Player p = new Player(record.FirstName, record.LastName, record.BirthDate, record.IsMale, record.Team,
                    record.StartNumber)
                { StartTime = record.StartTime };

                Distance distance;
                distancesDictionary.TryGetValue(record.StringDistance, out distance);

                ExtraPlayerInfo extraPlayerInfo;
                extraPlayerInfosDictionary.TryGetValue(record.StringAditionalInfo, out extraPlayerInfo);

                AgeCategory ageCategory;
                if (!ageCategoriesDictionary.TryGetValue(record.BirthDate.Year, out ageCategory))
                {
                    ageCategory = ageCategories
                        .FirstOrDefault(a => a.YearFrom <= record.BirthDate.Year && record.BirthDate.Year <= a.YearTo);
                    if (ageCategory != null)
                    {
                        ageCategoriesDictionary.Add(record.BirthDate.Year, ageCategory);
                    }
                }

                p = PreparePlayer(p, distance, extraPlayerInfo, ageCategory);

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