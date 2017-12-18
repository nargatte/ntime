using System;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class CompetitionRepositoryTests : RepositoryTests<Competition>
    {

        protected override Competition[] InitialItems { get; set; } =
        {
            new Competition("Zawody 1", new DateTime(1999, 11, 6), "Opis zawodów 1", "Zawody1.pl", "Organizator1", "Poznań"),
            new Competition("Zawody 2", new DateTime(2017, 11, 6), null, null, null, "Łódź"),
            new Competition("Zawody 3", new DateTime(2017, 11, 7), "Opis zawodów 3", null, null, "Warszawa"),
            new Competition("Zawody 4", new DateTime(2017, 12, 1), null, null, null, "Gdynia")
        };

        protected override Repository<Competition> Repository { get; set; }

        protected override Task BeforeDataSetUp(NTimeDBContext context)
        {
            Repository = new CompetitionRepository(ContextProvider);
            return base.BeforeDataSetUp(context);
        }

        private CompetitionRepository CompetitionRepository => (CompetitionRepository)Repository;

        protected override bool TheSameData(Competition entity1, Competition entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.City != entity2.City) return false;
            if (entity1.Description != entity2.Description) return false;
            if (entity1.EventDate != entity2.EventDate) return false;
            if (entity1.Link != entity2.Link) return false;
            if (entity1.Organizer != entity2.Organizer) return false;
            return true;
        }

        protected override bool SortTester(Competition before, Competition after) => before.EventDate > after.EventDate;
    }
}