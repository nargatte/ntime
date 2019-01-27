﻿using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateRepository : Repository<AgeCategoryTemplate>
    {
        public AgeCategoryTemplateRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<AgeCategoryTemplate> GetSortQuery(IQueryable<AgeCategoryTemplate> items) =>
            items.OrderBy(i => i.Name);

        public override Task RemoveAsync(AgeCategoryTemplate item)
        {
            CheckNull(item);
            CheckItem(item);

            return ContextProvider.DoAsync(async ctx =>
            {
                var ageCategoryTemplateItemsToRemove = ctx.AgeCategoryTemplateItems.Where(act => act.AgeCategoryCollectionId == item.Id);
                ctx.AgeCategoryTemplateItems.RemoveRange(ageCategoryTemplateItemsToRemove);

                ctx.AgeCategoryTemplates.Remove(item);
                await ctx.SaveChangesAsync();
            });

            //return base.RemoveAsync(item);
        }
    }
}