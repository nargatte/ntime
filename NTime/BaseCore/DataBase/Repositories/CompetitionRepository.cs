using System;
using System.Data.Entity;
using System.Linq;
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
                competition = (await ctx.Set<T>().FirstOrDefaultAsync(i => i.Id == relatedEntitieId)).Competition;
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

        public async Task<PageViewModel<Competition>> GetByAccountId(int accountId, PageBindingModel pageBindingModel)
        {
            PageViewModel<Competition> pageViewModel = new PageViewModel<Competition>();
            await ContextProvider.DoAsync(async ctx =>
            {
                pageViewModel.Items = await GetSortQuery(ctx.Set<Competition>().Where(c => c.Players.Any(p => p.PlayerAccount.AccountId == accountId))).Skip(pageBindingModel.ItemsOnPage * pageBindingModel.PageNumber).Take(pageBindingModel.ItemsOnPage).AsNoTracking<Competition>().ToArrayAsync();
                pageViewModel.TotalCount = await ctx.Set<Competition>().Where(c => c.Players.Any(p => p.PlayerAccount.AccountId == accountId)).CountAsync();
            });
            return pageViewModel;
        }
    }
}