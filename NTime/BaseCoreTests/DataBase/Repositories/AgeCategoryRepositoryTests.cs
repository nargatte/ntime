using System;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [TestFixture]
    public class AgeCategoryRepositoryCompetitionIdTests : RepositoryCompetitionIdTests<AgeCategory>
    {
        protected override AgeCategory[] InitialItems { get; set; } =
        {
            new AgeCategory("Młodziki", 2000, 2005),
            new AgeCategory("Starsi", 1995, 2000),
            new AgeCategory("Starszaki", 1985, 1990)
        };

        protected override Repository<AgeCategory> Repository => new AgeCategoryRepository(InitialCompetition);

        protected override bool TheSameData(AgeCategory entity1, AgeCategory entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.YearTo != entity2.YearTo) return false;
            if (entity1.YearFrom != entity2.YearFrom) return false;
            if (entity1.CompetitionId != entity2.CompetitionId) return false;
            return true;
        }

        protected override bool SortTester(AgeCategory before, AgeCategory after) => 
            String.CompareOrdinal(before.Name, after.Name) > 0;
    }
}