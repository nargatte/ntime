﻿using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class AgeCategoryTemplateRepositoryTests : RepositoryTests<AgeCategoryTemplate>
    {
        protected override AgeCategoryTemplate[] InitialItems { get; set; } =
        {
            new AgeCategoryTemplate("Młodziki", 2000, 2005, false),
            new AgeCategoryTemplate("Starsi", 1995, 2000, false),
            new AgeCategoryTemplate("Starszaki", 1985, 1990, false)
        };

        private AgeCategoryCollection InitialAgeCategoryCollection { get; set; } = new AgeCategoryCollection("Collection");

        protected override Repository<AgeCategoryTemplate> Repository { get; set; }

        private AgeCategoryTemplateItemRepository AgeCategoryTemplateRepository =>
            (AgeCategoryTemplateItemRepository) Repository;

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
            Repository = new AgeCategoryTemplateItemRepository(ContextProvider, InitialAgeCategoryCollection);
            InitialAgeCategoryCollection.AgeCategoryTemplates = null;
            ctx.AgeCategoryCollections.Add(InitialAgeCategoryCollection);
            await ctx.SaveChangesAsync();
        }

        protected override Task AfterDataTearDown(NTimeDBContext ctx)
        {
            return Task.Factory.StartNew(() => ctx.AgeCategoryCollections.RemoveRange(ctx.AgeCategoryCollections));
        }

        protected override void Reset(AgeCategoryTemplate item)
        {
            item.AgeCategoryCollection = null;
            item.AgeCategoryCollectionId = InitialAgeCategoryCollection.Id;
        }
    }
}