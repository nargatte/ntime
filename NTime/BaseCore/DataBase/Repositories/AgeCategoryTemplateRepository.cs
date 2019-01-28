using System.Data.Entity;
using System.Linq;
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
                var ageCategoryTemplateItemsToRemove = ctx.AgeCategoryTemplateItems.Where(act => act.AgeCategoryTemplateId == item.Id);
                ctx.AgeCategoryTemplateItems.RemoveRange(ageCategoryTemplateItemsToRemove);

                ctx.Entry(item).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            });

        }
    }
}