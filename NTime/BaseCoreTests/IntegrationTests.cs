using System;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using BaseCore.TimesProcess;
using NUnit.Framework;

namespace BaseCoreTests
{
    [TestFixture]
    public class IntegrationTests
    {
        private string pathToexport = "E:\\Downloads\\Export\\";

        [Test]
        public async Task LoadCsvs()
        {
            var cp = new ContextProvider();
            var cr = new CompetitionRepository(cp);
            var com = await cr.AddAsync(new Competition("Zawody", DateTime.Now, null, null, null, null));
            var pr = new PlayerRepository(cp, com);
            var eifr = new ExtraPlayerInfoRepository(cp, com);
            var dr = new DistanceRepository(cp, com);
            var akr = new AgeCategoryRepository(cp, com);
            var gr = new GateRepository(cp, com);

            await eifr.AddRangeAsync(new[]
            {
                new ExtraPlayerInfo("inny", "i"),
                new ExtraPlayerInfo("szosowy", "s")
            });

            await dr.AddRangeAsync(new[]
            {
                new Distance("MINI", 0, DateTime.Now, DistanceTypeEnum.DeterminedLaps) {LapsCount = 1},
                new Distance("+RODZINNY", 0, DateTime.Now, DistanceTypeEnum.DeterminedLaps) {LapsCount = 2},
                new Distance("MEGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedLaps) {LapsCount = 2},
                new Distance("RODZINNY", 0, DateTime.Now, DistanceTypeEnum.DeterminedLaps) {LapsCount = 1},
                new Distance("GIGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedDistance) {LapsCount = 3}
            });

            await akr.AddRangeAsync(new[]
            {
                new AgeCategory("Młodziki", 2001, 2005),
                new AgeCategory("Starsi", 1996, 2000),
                new AgeCategory("Starszaki", 1986, 1995),
                new AgeCategory("inny", 1900, 1985)
            });

            var g1 = await gr.AddAsync(new Gate("Pierwszy", 1));
            var g2 = await gr.AddAsync(new Gate("Drugi", 2));

            var darr = await dr.GetAllAsync();

            var ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "GIGA"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g2),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "MEGA"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g2),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "MINI"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g2),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "+RODZINNY"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "RODZINNY"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            TimeProcess timeProcess = new TimeProcess(com);

            await pr.ImportPlayersAsync(
                pathToexport + "Zawodnicy.csv");
            await timeProcess.ProcessAllAsync();
            await pr.ImportTimeReadsAsync(
                pathToexport + "log1.csv", g1);
            await timeProcess.ProcessAllAsync();
            await pr.ImportTimeReadsAsync(
                pathToexport + "log2.csv", g1);
            await timeProcess.ProcessAllAsync();
            await pr.ImportTimeReadsAsync(
                pathToexport + "log3.csv", g2);
            //await pr.ImportTimeReadsAsync(
            //  pathToexport + "log1 Lask.csv", g2);


            var pfo = new PlayerFilterOptions { Query = "500" };
            var p = (await pr.GetAllByFilterAsync(
                pfo, 0, 1)).Item1[0];

            //await timeProcess.ProcessSingleAsync(p);
            await timeProcess.ProcessAllAsync();

            await pr.UpdateFullCategoryAllAsync();
            await pr.UpdateRankingAllAsync();

            pfo = new PlayerFilterOptions
            {
                PlayerSort = PlayerSort.ByRank,
                CompleatedCompetition = true,
                HasVoid = false,
                WithoutStartTime = false,
                
            };
            //pfo.Query = "RODZ";
            var pall = await pr.GetAllByFilterAsync(
                pfo, 0, 5000);

            foreach (Player player in pall.Item1)
            {
                Console.WriteLine(player);
                var trr = new TimeReadRepository(cp, player);
                var ts = await trr.GetAllAsync();
            }
        }



    }
}