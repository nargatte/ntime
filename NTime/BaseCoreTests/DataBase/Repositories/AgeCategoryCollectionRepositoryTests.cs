using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class AgeCategoryCollectionRepositoryTests : RepositoryTests<AgeCategoryCollection>
    {
        protected override AgeCategoryCollection[] InitialItems { get; set; } =
        {
            new AgeCategoryCollection("Olimpiada"),
            new AgeCategoryCollection("Pływackie"),
            new AgeCategoryCollection("Co roczne, wiosenne, Śląskie"),
            new AgeCategoryCollection("Rowerowe Wrocławskie") 
        };

        protected override Repository<AgeCategoryCollection> Repository => new AgeCategoryCollectionRepository();

        protected override bool TheSameData(AgeCategoryCollection entity1, AgeCategoryCollection entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            return true;
        }

        protected override bool SortTester(AgeCategoryCollection before, AgeCategoryCollection after) => 
            String.CompareOrdinal(before.Name, after.Name) <= 0;
    }
}