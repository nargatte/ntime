using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public class PlayerAccountRepository : RepositoryAccountBase<PlayerAccount>
    {
        public PlayerAccountRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        public async Task<PageViewModel<PlayerAccount>> GetOrganizersByCompetition(Competition competition, PageBindingModel bindingModel)
        {
            PageViewModel<PlayerAccount> viewModel = new PageViewModel<PlayerAccount>();
            await ContextProvider.DoAsync(async ctx =>
            {
                viewModel.Items = await GetSortQuery(ctx.PlayerAccounts
                    .Where(pa => pa.Players.Any(p => p.Competition.Id == competition.Id)))
                    .Skip(bindingModel.ItemsOnPage * bindingModel.PageNumber).Take(bindingModel.ItemsOnPage)
                    .AsNoTracking().ToArrayAsync();
                viewModel.TotalCount = await ctx.PlayerAccounts
                    .Where(pa => pa.Players.Any(p => p.Competition.Id == competition.Id)).CountAsync();
            });
            return viewModel;
        }
    }
}