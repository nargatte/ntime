using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public class CompetitionRepository : Repository<Competition>
    {
        public CompetitionRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<Competition> GetSortQuery(IQueryable<Competition> items) =>
            items.OrderByDescending(e => e.EventDate);

        public async Task<Competition> GetByRelatedEntitieId<T>(int relatedEntitieId)
            where T: class, ICompetitionId, IEntityId
        {
            Competition competition = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                competition = (await ctx.Set<T>().FirstOrDefaultAsync(i => i.Id == relatedEntitieId))?.Competition;
            });
            return competition;
        }

        public async Task<Competition[]> GetOpens()
        {
            Competition[] competitions = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                competitions = await ctx.Competitions.Where(c => c.SignUpEndDate > DateTime.Now).ToArrayAsync();
            });
            return competitions;
        }

        public async Task<bool> CanOrganizerEdit(string accountId, Competition competition)
        {
            bool b = false;
            await ContextProvider.DoAsync(async ctx =>
            {
                b = await ctx.Competitions.Where(c => c.Id == competition.Id)
                    .AllAsync(c => c.OrganizerAccounts.Any(oc => oc.AccountId == accountId));
            });
            return b;
        }

        public async Task<Competition[]> GetCompetitionsByOrganizer(string accountId)
        {
            Competition[] competitions = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                competitions =
                    await GetSortQuery(ctx.Competitions.Where(c =>
                        c.OrganizerAccounts.Any(o => o.AccountId == accountId))).AsNoTracking().ToArrayAsync();
            });
            return competitions;
        }

        public Task<PageViewModel<Competition>> GetCompetitionsByPlayerAccount(PlayerAccount playerAccount,
            PageBindingModel pageBindingModel) =>
            PageTemplate<Competition>(pageBindingModel,
                e => GetSortQuery(e.Where(c => c.Players.Any(p => p.PlayerAccountId == playerAccount.Id))));
    }
}