using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class AgeCategoryRepository : RepositoryCompetitionId<AgeCategory>
    {
        public AgeCategoryRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<AgeCategory> GetSortQuery(IQueryable<AgeCategory> items) =>
            items.OrderBy(e => e.YearFrom);

        public async Task<AgeCategory> GetFitting(Player player)
        {
            AgeCategory ret = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                ret = await ctx.AgeCategories.FirstOrDefaultAsync(i => i.YearFrom <= player.BirthDate.Year && player.BirthDate.Year <= i.YearTo);
            });
            return ret;
        }
    }
}