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

            var darr = await dr.GetAllAsync();

            var ror = new GatesOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "GIGA"));
            await ror.ReplaceBy(new[]
            {
                new GatesOrder(1, 1800),
                new GatesOrder(2,1800),
                new GatesOrder(1,1800),
                new GatesOrder(2,1800),
                new GatesOrder(1,1800),
                new GatesOrder(2,1800),
                new GatesOrder(1,1800),
            });

            ror = new GatesOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "MEGA"));
            await ror.ReplaceBy(new[]
            {
                new GatesOrder(1, 1800),
                new GatesOrder(2,1800),
                new GatesOrder(1,1800),
            });

            ror = new GatesOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "MINI"));
            await ror.ReplaceBy(new[]
            {
                new GatesOrder(1,1800),
                new GatesOrder(2,1800),
                new GatesOrder(1,1800)
            });

            ror = new GatesOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "+RODZINNY"));
            await ror.ReplaceBy(new[]
            {
                new GatesOrder(1,1000),
                new GatesOrder(1,1000),
            });

            ror = new GatesOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "RODZINNY"));
            await ror.ReplaceBy(new[]
            {
                new GatesOrder(1,1000),
                new GatesOrder(1,1000)
            });

            await pr.ImportPlayersAsync(
                pathToexport + "Zawodnicy.csv");
            await pr.ImportTimeReadsAsync(
                pathToexport + "log1.csv", 1);
            await pr.ImportTimeReadsAsync(
                pathToexport + "log2.csv", 1);
            await pr.ImportTimeReadsAsync(
                pathToexport + "log3.csv", 2);
            // await pr.ImportTimeReadsAsync(
            //   pathToexport + "log1 Lask.csv", 2);


            var pfo = new PlayerFilterOptions { Query = "57" };
            var p = (await pr.GetAllByFilterAsync(
                pfo, 0, 1)).Item1[0];

            TimeProcess timeProcess = new TimeProcess(com);
            await timeProcess.ProcessSingleAsync(p);
            //await timeProcess.ProcessAllAsync();

            pfo = new PlayerFilterOptions
            {
                PlayerSort = PlayerSort.ByRank,
                CompleatedCompetition = true
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