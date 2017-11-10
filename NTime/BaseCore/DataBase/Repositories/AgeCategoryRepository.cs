using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class AgeCategoryRepository : RepositoryCompetitionId<AgeCategory>
    {
        public AgeCategoryRepository(Competition competition) : base(competition)
        {
        }

        protected override IQueryable<AgeCategory> GetSortQuery(IQueryable<AgeCategory> items) =>
            items.OrderBy(e => e.YearFrom);

        public async Task<AgeCategory> GetFitting(Player player)
        {
            AgeCategory ret = null;
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ret = await ctx.AgeCategories.FirstOrDefaultAsync(i => i.YearFrom <= player.BirthDate.Year && player.BirthDate.Year <= i.YearTo);
            });
            return ret;
        }
    }
}