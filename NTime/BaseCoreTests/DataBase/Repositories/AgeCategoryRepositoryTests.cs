using BaseCore.DataBase;

namespace BaseCoreTests.DataBase
{
    public class AgeCategoryRepositoryTests : RepositoryTestsBase<AgeCategory>
    {
        protected override AgeCategory[] InitialItems => new[]
        {
            new AgeCategory("Młodziki", 2000, 2005),
            new AgeCategory("Starsi", 1995, 2000),
            new AgeCategory("Starszaki", 1985, 1990)
        };

        protected override IRepository<AgeCategory> Repository => new AgeCategoryRepository();

        protected override bool TheSameData(AgeCategory entity1, AgeCategory entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.YearTo != entity2.YearTo) return false;
            if (entity1.YearFrom != entity2.YearFrom) return false;
            return true;
        }
    }
}