using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [TestFixture]
    public class AgeCategoryTemplateRepositoryTests : RepositoryTests<AgeCategoryTemplate>
    {
        protected override AgeCategoryTemplate[] InitialItems { get; set; } =
        {
            new AgeCategoryTemplate("Młodziki", 2000, 2005),
            new AgeCategoryTemplate("Starsi", 1995, 2000),
            new AgeCategoryTemplate("Starszaki", 1985, 1990)
        };

        private AgeCategoryCollection InitialAgeCategoryCollection { get; set; } = new AgeCategoryCollection("Collection");

        protected override Repository<AgeCategoryTemplate> Repository => new AgeCategoryTemplateRepository(InitialAgeCategoryCollection);

        protected override bool TheSameData(AgeCategoryTemplate entity1, AgeCategoryTemplate entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.YearTo != entity2.YearTo) return false;
            if (entity1.YearFrom != entity2.YearFrom) return false;
            if (entity1.AgeCategoryCollectionId != entity2.AgeCategoryCollectionId) return false;
            return true;
        }

        protected override bool SortTester(AgeCategoryTemplate before, AgeCategoryTemplate after) =>
            before.YearFrom < after.YearFrom;

        protected override async Task BeforeDataSetUp(NTimeDBContext ctx)
        {
            ctx.AgeCategoryCollections.Add(InitialAgeCategoryCollection);
            await ctx.SaveChangesAsync();
            Array.ForEach(InitialItems, i => i.AgeCategoryCollectionId = InitialAgeCategoryCollection.Id);
        }

        protected override Task AfterDataTearDown(NTimeDBContext ctx)
        {
            return Task.Factory.StartNew(() => ctx.AgeCategoryCollections.RemoveRange(ctx.AgeCategoryCollections));
        }
    }
}