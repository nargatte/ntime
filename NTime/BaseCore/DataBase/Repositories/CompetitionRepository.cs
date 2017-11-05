using System.Data.Entity;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class CompetitionRepository : Repository<Competition>
    {
        public async Task<Competition[]> GetAllAsync()
        {
            Competition[] competitions = null;
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                competitions = await ctx.Competitions.ToArrayAsync();
            });
            return competitions;
        }
    }
}