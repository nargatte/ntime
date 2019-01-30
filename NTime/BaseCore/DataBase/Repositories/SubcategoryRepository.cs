using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class SubcategoryRepository : RepositoryCompetitionId<Subcategory>
    {
        public SubcategoryRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<Subcategory> GetSortQuery(IQueryable<Subcategory> items) =>
            items.OrderBy(e => e.Name);

        public override Task RemoveAsync(Subcategory item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.Subcategory.Attach(item);
                await ctx.Players.Where(p => p.SubcategoryId == item.Id).ForEachAsync(p =>
                {
                    p.Subcategory = null;
                    p.SubcategoryId = null;
                });
                ctx.Subcategory.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }
    }
}