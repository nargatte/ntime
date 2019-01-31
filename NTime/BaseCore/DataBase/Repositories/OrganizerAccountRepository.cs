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

        protected override IQueryable<OrganizerAccount> GetSortQuery(IQueryable<OrganizerAccount> items) => items.OrderBy(item => item.AccountId);

        protected override IQueryable<OrganizerAccount> GetIncludeQuery(IQueryable<OrganizerAccount> items) =>
            items.Include(oa => oa.Account);

        //protected override IQueryable<OrganizerAccount> GetIncludeQuery(IQueryable<OrganizerAccount> items)
        //{
        //    return items.Include(account => account.);
        //}

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

        public Task SetCompetiton(OrganizerAccount account, Competition competition) =>
            ContextProvider.DoAsync(async ctx =>
            {
                bool b = await ctx.OrganizerAccounts.Where(o => o.AccountId == account.AccountId).AllAsync(o => o.Competitions.Any(c => c.Id == competition.Id));
                if (!b)
                {
                    OrganizerAccount ac =
                        await ctx.OrganizerAccounts.FirstOrDefaultAsync(o => o.AccountId == account.AccountId);

                    Competition co =
                        await ctx.Competitions.FirstOrDefaultAsync(c => c.Id == competition.Id);

                    ac.Competitions.Add(co);

                    await ctx.SaveChangesAsync();
                }
            });

        public Task UnsetCompetiton(OrganizerAccount account, Competition competition) =>
            ContextProvider.DoAsync(async ctx =>
            {
                bool b = await ctx.OrganizerAccounts.Where(o => o.AccountId == account.AccountId).AllAsync(o => o.Competitions.Any(c => c.Id == competition.Id));
                if (b)
                {
                    OrganizerAccount ac =
                        await ctx.OrganizerAccounts.FirstOrDefaultAsync(o => o.AccountId == account.AccountId);

                    Competition co =
                        await ctx.Competitions.FirstOrDefaultAsync(c => c.Id == competition.Id);

                    ac.Competitions.Remove(co);

                    await ctx.SaveChangesAsync();
                }
            });
    }
}