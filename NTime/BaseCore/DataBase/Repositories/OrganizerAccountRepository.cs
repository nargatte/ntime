using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class OrganizerAccountRepository : RepositoryAccountBase<OrganizerAccount>
    {
        public OrganizerAccountRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        public async Task<OrganizerAccount[]> GetOrganizersByCompetition(Competition competition)
        {
            OrganizerAccount[] organizerAccounts = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                organizerAccounts = await GetSortQuery(ctx.OrganizerAccounts
                    .Where(oa => oa.Competitions.Any(c => c.Id == competition.Id))).AsNoTracking().ToArrayAsync();
            });
            return organizerAccounts;
        }
    }
}