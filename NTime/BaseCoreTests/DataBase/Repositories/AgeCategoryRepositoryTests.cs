using System;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace BaseCoreTests.DataBase
{
    public class AgeCategoryRepositoryTests : RepositoryCompetitionIdTests<AgeCategory>
    {
        protected override AgeCategory[] InitialItems { get; set; } =
        {
            new AgeCategory("Młodziki", 2000, 2005, false),
            new AgeCategory("Starsi", 1995, 2000, false),
            new AgeCategory("Starszaki", 1985, 1990, false)
        };

        protected override Repository<AgeCategory> Repository { get; set; }

        protected override Task BeforeDataSetUp(NTimeDBContext ctx)
        {
            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(ContextProvider, InitialCompetition);
            Repository = ageCategoryRepository;
            return base.BeforeDataSetUp(ctx);
        }

        protected override bool TheSameData(AgeCategory entity1, AgeCategory entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.YearTo != entity2.YearTo) return false;
            if (entity1.YearFrom != entity2.YearFrom) return false;
            if (entity1.CompetitionId != entity2.CompetitionId) return false;
            return true;
        }

        protected override bool SortTester(AgeCategory before, AgeCategory after) =>
            before.YearFrom < after.YearFrom;
    }
}