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


        //protected override IQueryable<T> GetSortQuery(IQueryable<T> items) =>
        //    items.OrderBy(e => e.AccountId);

        public async Task<PageViewModel<T>> GetByQuery(string query, PageBindingModel pageBindingModel)
        {

            if (query == null)
                return await GetAllAsync(pageBindingModel);

            PageViewModel<T> pageViewModel = await PageTemplate<T>(pageBindingModel,
                a => GetSortQuery(a.OrderBy(aa => aa.AccountId)));

            return pageViewModel;
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