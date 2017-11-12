using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using NUnit.Framework;

namespace BaseCoreTests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public async Task LoadCsvs()
        {
            var cp =  new ContextProvider("Integration");
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
                new Distance("MINI", 0, DateTime.Now, DistanceTypeEnum.DeterminedCircuits),
                new Distance("+RODZINNY", 0, DateTime.Now, DistanceTypeEnum.DeterminedCircuits),
                new Distance("MEGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedCircuits),
                new Distance("RODZINNY", 0, DateTime.Now, DistanceTypeEnum.DeterminedCircuits),
                new Distance("GIGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedCircuits)
            });

            await akr.AddRangeAsync(new[]
            {
                new AgeCategory("Młodziki", 2001, 2005),
                new AgeCategory("Starsi", 1996, 2000),
                new AgeCategory("Starszaki", 1986, 1995),
                new AgeCategory("inny", 1900, 1985)
            });

            await pr.ImportPlayersAsync(
                "C:\\Users\\nargatte\\Documents\\projects\\ntime\\NTime\\BaseCoreTests\\Csv\\TestFiles\\Players.csv");
            await pr.ImportTimeReadsAsync(
                "C:\\Users\\nargatte\\Documents\\projects\\ntime\\NTime\\BaseCoreTests\\Csv\\TestFiles\\Times.csv", 1);
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