using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public class RepositoryAccountBase<T> : Repository<T>
        where T: class, IFullName, IEntityId, IAccountId
    {
        public RepositoryAccountBase(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<T> GetSortQuery(IQueryable<T> items) =>
            items.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ThenBy(e => e.AccountId);

        public async Task<PageViewModel<T>> GetByQuery(string query, PageBindingModel pageBindingModel)
        {
            if (query == null)
                return await GetAllAsync(pageBindingModel);
            PageViewModel<T> pageViewModel = new PageViewModel<T>();
            await ContextProvider.DoAsync(async ctx =>
            {
                pageViewModel.Items = await GetSortQuery(ctx.Set<T>().Where(e => e.FirstName.StartsWith(query) || e.LastName.StartsWith(query))).Skip(pageBindingModel.ItemsOnPage * pageBindingModel.PageNumber).Take(pageBindingModel.ItemsOnPage).AsNoTracking<T>().ToArrayAsync();
                pageViewModel.TotalCount = await GetAllQuery(ctx.Set<T>()).CountAsync();
            });
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