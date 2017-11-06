using System;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [TestFixture]
    public class CompetitionRepositoryTests : RepositoryTestsBase<Competition>
    {

        protected override Competition[] InitialItems => new[]
        {
            new Competition("Zawody 1", new DateTime(2017, 11, 6), "Opis zawodów 1", "Zawody1.pl", "Organizator1", "Poznań", CompetitionTypeEnum.Fastest ),
            new Competition("Zawody 2", new DateTime(2017, 11, 6), null, null, null, "Łódź", CompetitionTypeEnum.MostLaps ),
            new Competition("Zawody 3", new DateTime(2017, 11, 6), "Opis zawodów 3", null, null, "Warszawa", CompetitionTypeEnum.Fastest ),
            new Competition("Zawody 4", new DateTime(2017, 12, 1), null, null, null, "Gdynia", CompetitionTypeEnum.Fastest )
        };

        protected override IRepository<Competition> Repository => new CompetitionRepository();
        private CompetitionRepository CompetitionRepository => (CompetitionRepository)Repository;

        protected override bool TheSameData(Competition entity1, Competition entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.CompetitionTypeEnum != entity2.CompetitionTypeEnum) return false;
            if (entity1.City != entity2.City) return false;
            if (entity1.Description != entity2.Description) return false;
            if (entity1.EventDate != entity2.EventDate) return false;
            if (entity1.Link != entity2.Link) return false;
            if (entity1.Organiser != entity2.Organiser) return false;
            return true;
        }

        [Test]
        public async Task GetAllTest()
        {
            Competition[] competitions = await CompetitionRepository.GetAllAsync();
            Assert.IsTrue(TheSameDataArrays(competitions, InitialItems, TheSameData));
        }
    }
}