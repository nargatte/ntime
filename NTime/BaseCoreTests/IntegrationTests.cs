using System;
using System.Security;
using System.Threading.Tasks;
using BaseCore.DataBase;
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
                new Distance("GIGA", 0, DateTime.Now, DistanceTypeEnum.DeterminedCircuits),
            });
            await pr.ImportPlayersAsync(
                "C:\\Users\\nargatte\\Documents\\projects\\ntime\\NTime\\BaseCoreTests\\Csv\\TestFiles\\Players.csv");
            await pr.ImportTimeReadsAsync(
                "C:\\Users\\nargatte\\Documents\\projects\\ntime\\NTime\\BaseCoreTests\\Csv\\TestFiles\\Times.csv", 1);

        }
    }
}