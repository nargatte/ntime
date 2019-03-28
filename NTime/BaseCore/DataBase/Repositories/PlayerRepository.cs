using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BaseCore.Csv;
using BaseCore.Models;
using BaseCore.PlayerFilter;
using BaseCore.TimesProcess;

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
            items.Include(p => p.Distance).Include(p => p.AgeCategory).Include(p => p.Subcategory);

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
            var competitionRepository = new CompetitionRepository(new ContextProvider());
            Player[] players = null;
            int totalItemsNumber = 0;

            if (filterOptions.PlayerSort == PlayerSort.ByExtraData)
            {
                filterOptions.PlayerSort = PlayerSort.ByFirstName;
                pageNumber = 0;
                numberItemsOnPage = int.MaxValue;
                char delimiter = competitionRepository.DelimiterForExtraData;

                await ContextProvider.DoAsync(async ctx =>
                {
                    players = await GetIncludeQuery(GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                        filterOptions).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage)).AsNoTracking().ToArrayAsync();

                    totalItemsNumber = await GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                        filterOptions).CountAsync();
                });
                try
                {
                    if (filterOptions.DescendingSort)
                        players = players.OrderByDescending(p =>
                            (p.ExtraData ?? String.Empty).Split(new[] { delimiter }, StringSplitOptions.None)[filterOptions.ExtraDataSortIndex]).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage).ToArray();
                    else
                        players = players.OrderBy(p =>
                            (p.ExtraData ?? String.Empty).Split(new[] { delimiter }, StringSplitOptions.None)[filterOptions.ExtraDataSortIndex]).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage).ToArray();
                }
                catch (IndexOutOfRangeException)
                {
                    throw new ArgumentException("Podana kolumna sortowania jest nieprawidłowa");
                }
            }
            else
            {
                await ContextProvider.DoAsync(async ctx =>
                {
                    players = await GetIncludeQuery(GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                        filterOptions).Skip(pageNumber * numberItemsOnPage).Take(numberItemsOnPage)).AsNoTracking().ToArrayAsync();

                    totalItemsNumber = await GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                        filterOptions).CountAsync();
                });
            }

            return new Tuple<Player[], int>(players, totalItemsNumber);
        }

        public async Task<PageViewModel<Player>> GetAllByFilterAsync(PlayerFilterOptions filterOptions, PageBindingModel bindingModel)
        {
            Tuple<Player[], int> tuple =
                await GetAllByFilterAsync(filterOptions, bindingModel.PageNumber, bindingModel.ItemsOnPage);
            return new PageViewModel<Player>
            {
                Items = tuple.Item1,
                TotalCount = tuple.Item2
            };
        }

        private IQueryable<Player> GetFilteredQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions) =>
            GetSortQuery(GetStringQuery(GetWhereQuery(items, filterOptions), filterOptions), filterOptions);

        private IQueryable<Player> GetWhereQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions)
        {
            if (filterOptions.AgeCategory != null)
                items = items.Where(i => i.AgeCategoryId == filterOptions.AgeCategory.Id);

            if (filterOptions.Distance != null)
                items = items.Where(i => i.DistanceId == filterOptions.Distance.Id);

            if (filterOptions.Subcategory != null)
                items = items.Where(i => i.SubcategoryId == filterOptions.Subcategory.Id);

            if (filterOptions.Men != null)
                items = items.Where(i => i.IsMale == filterOptions.Men);

            if (filterOptions.WithoutStartTime != null)
                items = items.Where(i => (i.IsStartTimeFromReader || i.StartTime == null) ^ !filterOptions.WithoutStartTime.Value);

            if (filterOptions.Invalid != null)
                items = items.Where(i => (i.AgeCategoryId == null || i.DistanceId == null ||
                                         i.SubcategoryId == null) ^ !filterOptions.Invalid.Value);

            if (filterOptions.CompletedCompetition != null)
                items = items.Where(i => i.CompetitionCompleted == filterOptions.CompletedCompetition);

            if (filterOptions.HasVoid != null)
                items = items.Where(i => i.TimeReads.All(t => t.TimeReadTypeId != (int)TimeReadTypeEnum.Void) !=
                                         filterOptions.HasVoid);
            if (filterOptions.IsPaidUp != null)
                items = items.Where(i => i.IsPaidUp == filterOptions.IsPaidUp);

            return items;
        }

        private IQueryable<Player> GetStringQuery(IQueryable<Player> items, PlayerFilterOptions filterOptions)
        {

            if (filterOptions.Query == null)
                return items;

            string[] numberStrings = filterOptions.Query.Split('-');
            if (numberStrings.Length == 2 && Int32.TryParse(numberStrings[0], out int oneNumenr) &&
                Int32.TryParse(numberStrings[1], out int twoNumber))
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
                i.Team.Contains(s) || i.FullCategory.Contains(s) || i.ExtraData.Contains(s));

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

                case PlayerSort.ByExtraData:
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

        public async Task<Player> AddAsync(Player player, AgeCategory ageCategory, Distance distance, Subcategory subcategory)
        {
            await AddAsync(PreparePlayer(player, distance, subcategory, ageCategory));
            player.Distance = distance;
            player.Subcategory = subcategory;
            player.AgeCategory = ageCategory;
            return player;
        }

        public async Task UpdateAsync(Player player, AgeCategory ageCategory, Distance distance, Subcategory subcategory)
        {
            player.Distance = distance;
            player.DistanceId = distance?.Id;
            player.Subcategory = subcategory;
            player.SubcategoryId = subcategory?.Id;
            player.AgeCategory = ageCategory;
            player.AgeCategoryId = ageCategory?.Id;
            player.FullCategory = GetFullCategory(distance, subcategory, ageCategory, player.IsMale);
            CheckNull(player);
            CheckItem(player);
            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.Players.AddOrUpdate(player);

                await ctx.SaveChangesAsync();
            });
        }

        public Task UpdateFullCategoryAllAsync()
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                Player[] players = await ctx.Players.Where(p => p.CompetitionId == Competition.Id && p.IsCategoryFixed == false).Include(p => p.AgeCategory).ToArrayAsync();
                AgeCategory[] categories = await ctx.AgeCategories.Where(a => a.CompetitionId == Competition.Id)
                        .ToArrayAsync();

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

                    player.FullCategory = GetFullCategory(player.Distance, player.Subcategory, player.AgeCategory,
                        player.IsMale);
                }

                await ctx.SaveChangesAsync();
            });


        }

        public async Task UpdateRankingAllAsync() =>
            await ContextProvider.DoAsync(async ctx =>
            {
                AgeCategory[] ageCategories = await ctx.AgeCategories.Where(a => a.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                Subcategory[] subcategories = await ctx.Subcategory.Where(a => a.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                Distance[] distances = await ctx.Distances.Where(a => a.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();

                PlayerFilterOptions filterOptions = new PlayerFilterOptions()
                {
                    PlayerSort = PlayerSort.ByRank,
                    WithoutStartTime = false,
                    Invalid = false,
                    HasVoid = false,
                    CompletedCompetition = true,
                    DescendingSort = false
                };

                for (int ism = 0; ism <= 1; ism++)
                {
                    filterOptions.Men = ism != 0;
                    foreach (Distance distance in distances)
                    {
                        filterOptions.Distance = distance;
                        filterOptions.AgeCategory = null;
                        filterOptions.Subcategory = null;
                        OrderSet(GetFilteredQuery(ctx.Players.Where(i => i.CompetitionId == Competition.Id),
                            filterOptions), (i, player) => player.DistancePlaceNumber = i + 1);
                        foreach (Subcategory subcategory in subcategories)
                        {
                            foreach (AgeCategory ageCategory in ageCategories)
                            {
                                filterOptions.AgeCategory = ageCategory;
                                filterOptions.Subcategory = subcategory;
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

        private async Task<Player> PreparePlayer(Player player, Distance distance, Subcategory subcategory)
        {
            PrepareToAdd(player);

            player.DistanceId = distance?.Id;
            player.Distance = null;

            player.SubcategoryId = subcategory?.Id;
            player.Subcategory = null;

            AgeCategory ageCategory = (await (new AgeCategoryRepository(ContextProvider, Competition)).GetFittingAsync(player))[0];
            player.AgeCategoryId = ageCategory?.Id;
            player.AgeCategory = null;

            player.FullCategory = GetFullCategory(distance, subcategory, ageCategory, player.IsMale);
            return player;
        }

        private Player PreparePlayer(Player player, Distance distance, Subcategory subcategory, AgeCategory ageCategory)
        {
            PrepareToAdd(player);

            player.DistanceId = distance?.Id;
            player.Distance = null;

            player.SubcategoryId = subcategory?.Id;
            player.Subcategory = null;

            player.AgeCategoryId = ageCategory?.Id;
            player.AgeCategory = null;

            player.FullCategory = GetFullCategory(distance, subcategory, ageCategory, player.IsMale);
            return player;
        }

        protected virtual string GetFullCategory(Distance distance, Subcategory subcategory, AgeCategory ageCategory,
            bool male) =>
            (distance == null || subcategory == null || ageCategory == null) ? null :
            distance.Name.Substring(0, Math.Min(4, distance.Name.Length)) + " " + ageCategory.Name + (subcategory.ShortName == "*" ? "" : subcategory.ShortName);

        public async Task<Tuple<int, int>> ImportTimeReadsAsync(string fileName, Gate gate)
        {
            char delimiter = ';';
            CsvImporter<TimeReadRecord, TimeReadRecordMap> csvImporter = new CsvImporter<TimeReadRecord, TimeReadRecordMap>(fileName, delimiter);

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
                if (dictionaryPlayers.TryGetValue(read.StartNumber, out Player p))
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

        public async Task<(int playersCount, int playerRecordsCount)> ImportPlayersAsync(string fileName)
        {
            CsvImporter<PlayerRecord, PlayerRecordMap> csvImporter = new CsvImporter<PlayerRecord, PlayerRecordMap>(fileName, ';');

            PlayerRecord[] playerRecords = await csvImporter.GetAllRecordsAsync();

            Distance[] distances = null;
            Subcategory[] subcategories = null;
            AgeCategory[] ageCategories = null;

            await ContextProvider.DoAsync(async ctx =>
            {
                distances = await ctx.Distances.Where(d => d.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                subcategories = await ctx.Subcategory.Where(e => e.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
                ageCategories = await ctx.AgeCategories.Where(e => e.CompetitionId == Competition.Id).AsNoTracking().ToArrayAsync();
            });

            var distancesDictionary = new Dictionary<string, Distance>();
            var subcategoriesDictionary = new Dictionary<string, Subcategory>();
            var ageCategoriesDictionary = new Dictionary<int, AgeCategory>();

            foreach (var distance in distances)
            {
                distancesDictionary.Add(distance.Name, distance);
            }

            foreach (var subcategory in subcategories)
            {
                subcategoriesDictionary.Add(subcategory.Name, subcategory);
            }

            var players = new List<Player>();

            foreach (var record in playerRecords)
            {
                if (record.StartNumber < 0) continue;
                var importedPlayer = new Player(record.FirstName, record.LastName, record.BirthDate, record.IsMale, record.Team,
                    record.StartNumber)
                { StartTime = record.StartTime, ExtraData = record.ExtraData };

                distancesDictionary.TryGetValue(record.StringDistance, out Distance distance);

                subcategoriesDictionary.TryGetValue(record.StringAditionalInfo, out Subcategory subcategory);

                if (!ageCategoriesDictionary.TryGetValue(record.BirthDate.Year, out AgeCategory ageCategory))
                {
                    ageCategory = ageCategories
                        .FirstOrDefault(a => a.YearFrom <= record.BirthDate.Year && record.BirthDate.Year <= a.YearTo);
                    if (ageCategory != null)
                    {
                        ageCategoriesDictionary.Add(record.BirthDate.Year, ageCategory);
                    }
                }

                importedPlayer = PreparePlayer(importedPlayer, distance, subcategory, ageCategory);

                players.Add(importedPlayer);
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.Players.AddRange(players);
                await ctx.SaveChangesAsync();
            });

            return (players.Count, playerRecords.Length);
        }

        public Task RemoveAllTimeReads() =>
            ContextProvider.DoAsync(async ctx =>
            {
                ctx.TimeReads.RemoveRange(ctx.TimeReads.Where(t => t.Gate.CompetitionId == Competition.Id));
                await ctx.SaveChangesAsync();
            });


        public async Task ImportTimeReadsFromSourcesAsync()
        {
            await RemoveAllTimeReads();
            Gate[] gates = await new GateRepository(ContextProvider, Competition).GetAllAsync();
            //List<Task> tasks = new List<Task>();
            foreach (Gate gate in gates)
            {
                TimeReadsLogInfo[] infos = await new TimeReadsLogInfoRepository(ContextProvider, gate).GetAllAsync();
                foreach (TimeReadsLogInfo info in infos)
                {
                    await ImportTimeReadsAsync(info.Path, gate);
                }
            }
            //await Task.WhenAll(tasks);
        }

        public async Task<bool> CanPlayerEdit(string accountId, Player player)
        {
            bool b = false;
            await ContextProvider.DoAsync(async ctx =>
            {
                b = await ctx.Players.Where(p => p.Id == player.Id).AllAsync(p => p.PlayerAccountId != null && p.PlayerAccount.AccountId == accountId);
            });
            return b;
        }

        public async Task<bool> IsNowRegister(string accountId)
        {
            bool b = false;
            await ContextProvider.DoAsync(async ctx =>
            {
                b = await ctx.Players.AnyAsync(p =>
                    p.CompetitionId == Competition.Id && p.PlayerAccount.AccountId == accountId);
            });
            return b;
        }

        public async Task<Player> GetFromPlayerAccountFromCompetition(PlayerAccount playerAccount,
            Competition competition)
        {
            Player player = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                player = await ctx.Players.AsNoTracking()
                    .FirstOrDefaultAsync(
                        p => p.CompetitionId == Competition.Id && p.PlayerAccountId == playerAccount.Id);
            });
            return player;
        }

        public Task ModifyExtraData(int[] permutations, char delimiter)
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                var players = await ctx.Players.Where(p => p.CompetitionId == Competition.Id).ToArrayAsync();

                foreach (var player in players)
                {
                    if (string.IsNullOrWhiteSpace(player.ExtraData))
                        player.ExtraData = String.Concat(Enumerable.Repeat(delimiter, permutations.Length - 1).ToArray());
                    else
                    {
                        string[] extraData = player.ExtraData.Split(new[] { delimiter }, StringSplitOptions.None);
                        string[] newExtraData = new string[permutations.Length];
                        for (int x = 0; x < newExtraData.Length; x++)
                        {
                            if (permutations[x] >= 0 && permutations[x] < extraData.Length)
                                newExtraData[x] = extraData[permutations[x]];
                            else
                                newExtraData[x] = String.Empty;
                        }

                        player.ExtraData = String.Join(delimiter.ToString(), newExtraData);
                    }
                }

                await ctx.SaveChangesAsync();
            });
        }

        public async Task<MemoryStream> ExportPlayersToCsv()
        {
            char initialSeparator = '|';
            char targetSeperator = ';';

            var players = await this.GetAllAsync();

            var builder = new StringBuilder();
            var csvHeaders = @"nr_startowy;imie;nazwisko;miejscowosc;klub;data_urodzenia;plec;telefon;pay;czas_startu;kategoria;kat_wiek;nazwa_dystansu";
            if (!String.IsNullOrWhiteSpace(Competition.ExtraDataHeaders))
            {
                var extraHeaders = Competition.ExtraDataHeaders.Split(initialSeparator);
                var extraHeadersToCsv = String.Join(targetSeperator.ToString(), extraHeaders);
                csvHeaders += $"{targetSeperator}{extraHeadersToCsv}";
            }
            builder.AppendLine(csvHeaders);

            foreach (Player p in players)
            {
                string birthDate = p.BirthDate.ToString("yyyy-MM-dd");
                string sex = p.IsMale ? "M" : "K";
                string pay = p.IsPaidUp ? "1" : "";

                var csvPlayerData = $"{p.StartNumber};{p.FirstName};{p.LastName};{p.City ?? ""};{p.Team ?? ""};{birthDate};{sex};{p.PhoneNumber ?? ""};{pay};{p.StartTime?.ToString("T") ?? ""};{p.FullCategory ?? ""};{p.AgeCategory?.Name ?? ""};{p.Distance?.Name ?? ""}";
                if (!String.IsNullOrWhiteSpace(p.ExtraData))
                {
                    var extraData = p.ExtraData.Split(initialSeparator);
                    var extraDataToCsv = String.Join(targetSeperator.ToString(), extraData);
                    csvPlayerData += $"{targetSeperator}{extraDataToCsv}";
                }
                builder.AppendLine(csvPlayerData);
            }

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(builder.ToString());
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}