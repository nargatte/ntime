using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public class RepositoryAccountBase<T> : Repository<T>
        where T: class, IEntityId, IAccountId
    {
        public RepositoryAccountBase(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        public async Task<T> GetByAccountId(string accountId)
        {
            T entity = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                entity = await ctx.Set<T>().FirstOrDefaultAsync(e => e.AccountId == accountId);
            });
            return entity;
        }
    }
}