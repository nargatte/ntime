using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class AgeCategoryTemplateRepositoryTests : RepositoryTests<AgeCategoryTemplateItem>
    {
        protected override AgeCategoryTemplateItem[] InitialItems { get; set; } =
        {
            new AgeCategoryTemplateItem("Młodziki", 2000, 2005, false),
            new AgeCategoryTemplateItem("Starsi", 1995, 2000, false),
            new AgeCategoryTemplateItem("Starszaki", 1985, 1990, false)
        };

        private AgeCategoryTemplate InitialAgeCategoryCollection { get; set; } = new AgeCategoryTemplate("Collection");

        protected override Repository<AgeCategoryTemplateItem> Repository { get; set; }

        private AgeCategoryTemplateItemRepository AgeCategoryTemplateRepository =>
            (AgeCategoryTemplateItemRepository) Repository;

        protected override bool TheSameData(AgeCategoryTemplateItem entity1, AgeCategoryTemplateItem entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.YearTo != entity2.YearTo) return false;
            if (entity1.YearFrom != entity2.YearFrom) return false;
            if (entity1.AgeCategoryCollectionId != entity2.AgeCategoryCollectionId) return false;
            return true;
        }

        protected override bool SortTester(AgeCategoryTemplateItem before, AgeCategoryTemplateItem after) =>
            before.YearFrom < after.YearFrom;

        protected override async Task BeforeDataSetUp(NTimeDBContext ctx)
        {
            Repository = new AgeCategoryTemplateItemRepository(ContextProvider, InitialAgeCategoryCollection);
            InitialAgeCategoryCollection.AgeCategoryTemplates = null;
            ctx.AgeCategoryTemplates.Add(InitialAgeCategoryCollection);
            await ctx.SaveChangesAsync();
        }

        protected override Task AfterDataTearDown(NTimeDBContext ctx)
        {
            return Task.Factory.StartNew(() => ctx.AgeCategoryTemplates.RemoveRange(ctx.AgeCategoryTemplates));
        }

        protected override void Reset(AgeCategoryTemplateItem item)
        {
            item.AgeCategoryCollection = null;
            item.AgeCategoryCollectionId = InitialAgeCategoryCollection.Id;
        }
    }
}