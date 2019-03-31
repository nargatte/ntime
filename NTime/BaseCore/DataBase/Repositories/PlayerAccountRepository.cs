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

        public Task<PageViewModel<PlayerAccount>> GetPlayersAccountsByCompetition(Competition competition,
            PageBindingModel bindingModel) =>
            PageTemplate<PlayerAccount>(bindingModel,
                e => GetSortQuery(e.Where(pa => pa.Players.Any(p => p.Competition.Id == competition.Id))));

        public Task<PageViewModel<Player>> GetPlayersByPlayerAccount(string accountId, PageBindingModel bindingModel) =>
            PageTemplate<Player>(bindingModel,
                e => e.Where(p => p.PlayerAccount.AccountId == accountId).OrderByDescending(p => p.Competition.EventDate));


    }
}