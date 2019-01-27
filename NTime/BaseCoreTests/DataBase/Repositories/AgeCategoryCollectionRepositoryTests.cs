using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class AgeCategoryCollectionRepositoryTests : RepositoryTests<AgeCategoryTemplate>
    {
        protected override AgeCategoryTemplate[] InitialItems { get; set; } =
        {
            new AgeCategoryTemplate("Olimpiada"),
            new AgeCategoryTemplate("Pływackie"),
            new AgeCategoryTemplate("Co roczne, wiosenne, Śląskie"),
            new AgeCategoryTemplate("Rowerowe Wrocławskie") 
        };

        protected override Repository<AgeCategoryTemplate> Repository { get; set; }

        protected override Task BeforeDataSetUp(NTimeDBContext context)
        {
            Repository = new AgeCategoryTemplateRepository(ContextProvider);
            return base.BeforeDataSetUp(context);
        }

        protected override bool TheSameData(AgeCategoryTemplate entity1, AgeCategoryTemplate entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            return true;
        }

        protected override bool SortTester(AgeCategoryTemplate before, AgeCategoryTemplate after) => 
            String.CompareOrdinal(before.Name, after.Name) <= 0;
    }
}