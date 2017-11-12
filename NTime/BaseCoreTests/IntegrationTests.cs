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
            var cp =  new ContextProvider();
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
                new Distance("MINI", 0, DateTime.Now, DistanceTypeEnum.DeterminedDistance),
                new Distance("+RODZINNY", 0, DateTime.Now, DistanceTypeEnum.DeterminedDistance),
                new Distance("MEGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedDistance),
                new Distance("RODZINNY", 0, DateTime.Now, DistanceTypeEnum.DeterminedDistance),
                new Distance("GIGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedDistance)
            });

            await akr.AddRangeAsync(new[]
            {
                new AgeCategory("Młodziki", 2001, 2005),
                new AgeCategory("Starsi", 1996, 2000),
                new AgeCategory("Starszaki", 1986, 1995),
                new AgeCategory("inny", 1900, 1985)
            });

            var darr = await dr.GetAllAsync();

            var ror = new ReaderOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "GIGA"));
            await ror.ReplaceBy(new[]
            {
                new ReaderOrder(1, 0),
                new ReaderOrder(2, 0),
                new ReaderOrder(1, 0),
                new ReaderOrder(2, 0),
                new ReaderOrder(1, 0),
                new ReaderOrder(2, 0),
                new ReaderOrder(1, 0)
            });

            ror = new ReaderOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "MEGA"));
            await ror.ReplaceBy(new[]
            {
                new ReaderOrder(1, 0),
                new ReaderOrder(2, 0),
                new ReaderOrder(1, 0),
                new ReaderOrder(2, 0),
                new ReaderOrder(1, 0)
            });

            ror = new ReaderOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "MINI"));
            await ror.ReplaceBy(new[]
            {
                new ReaderOrder(1, 0),
                new ReaderOrder(2, 0),
                new ReaderOrder(1, 0)
            });

            ror = new ReaderOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "+RODZINNY"));
            await ror.ReplaceBy(new[]
            {
                new ReaderOrder(1, 0),
                new ReaderOrder(1, 0),
                new ReaderOrder(1, 0)
            });

            ror = new ReaderOrderRepository(cp, darr.FirstOrDefault(d => d.Name == "RODZINNY"));
            await ror.ReplaceBy(new[]
            {
                new ReaderOrder(1, 0),
                new ReaderOrder(1, 0)
            });

            await pr.ImportPlayersAsync(
                pathToexport + "Zawodnicy.csv");
            await pr.ImportTimeReadsAsync(
                pathToexport + "log1.csv", 1); // 1
            await pr.ImportTimeReadsAsync(
                pathToexport + "log2.csv", 1); //1
            await pr.ImportTimeReadsAsync(
                pathToexport + "log3.csv", 2); //2
           // await pr.ImportTimeReadsAsync(
             //   pathToexport + "log1 Lask.csv", 2);

            var p = (await pr.GetAllByFilterAsync(
                new PlayerFilterOptions(PlayerSort.ByFirstName, false, "320", null, null, null, null, null,
                    null), 0, 1)).Item1[0];

            TimeProcess timeProcess = new TimeProcess(com);
            //await timeProcess.ProcessSingleAsync(p);
            await timeProcess.ProcessAllAsync();

            var pall = await pr.GetAllByFilterAsync(
                new PlayerFilterOptions(PlayerSort.ByRank, false, null, null, null, null, null, null, null), 0, 50);

            foreach ( Player player in pall.Item1)
            {
                var trr = new TimeReadRepository(cp, player);
                var ts = await trr.GetAllAsync();
            }
        }

        [Test]
        public async Task Filters()
        {
            var cp = new ContextProvider("Integration");
            var com = (await new CompetitionRepository(cp).GetAllAsync())[0];
            var pr = new PlayerRepository(cp, com);

            var t = await pr.GetAllByFilterAsync(new PlayerFilterOptions(PlayerSort.ByBirthDate, false, "30 - 60", null, null, null, null, null, null),0, 30);
        }



    }
}