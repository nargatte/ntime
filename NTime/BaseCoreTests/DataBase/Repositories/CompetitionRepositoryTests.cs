using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [TestFixture]
    public class CompetitionRepositoryTests : RepositoryTestsBase
    {
        private readonly CompetitionRepository _competitionRepository = new CompetitionRepository();

        private readonly List<Competition> _competitions = new List<Competition>()
        {
            new Competition("Zawody 1", new DateTime(2017, 11, 6), null, null, null, null, CompetitionTypeEnum.Fastest ),
            new Competition("Zawody 2", new DateTime(2017, 11, 6), null, null, null, null, CompetitionTypeEnum.MostLaps ),
            new Competition("Zawody 3", new DateTime(2017, 11, 6), "Opis zawodów 3", null, null, null, CompetitionTypeEnum.Fastest ),
            new Competition("Zawody 4", new DateTime(2017, 12, 1), null, null, null, null, CompetitionTypeEnum.Fastest )
        };

        [SetUp]
        public async Task DataSetUp()
        {
            await _competitionRepository.AddRangeAsync(_competitions);
        }

        [Test]
        public async Task AllCheck()
        {
            Competition[] competitions = await _competitionRepository.GetAllAsync();
            Assert.IsNotNull(competitions);
        }
    }
}