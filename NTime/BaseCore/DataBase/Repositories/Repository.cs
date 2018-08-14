using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.Models;

namespace BaseCore.DataBase
{
    public abstract class Repository<T>
        where T : class, IEntityId
    {
        protected IContextProvider ContextProvider;

        protected Repository(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public async Task<T> AddAsync(T item)
        {
            CheckNull(item);
            PrepareToAdd(item);
            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.Set<T>().Add(item);
                await ctx.SaveChangesAsync();
            });
            return item;
        }

        public Task AddRangeAsync(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                CheckNull(item);
                PrepareToAdd(item);
            }

            return ContextProvider.DoAsync(async ctx =>
            {
                foreach (T item in items)
                    ctx.Entry(item).State = EntityState.Added;
                await ctx.SaveChangesAsync();
            });
        }

        public virtual Task UpdateAsync(T item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            });

        }

        public virtual Task UpdateRangeAsync(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                CheckNull(item);
                CheckItem(item);
            }
            return ContextProvider.DoAsync(async ctx =>
            {
                foreach (T item in items)
                    ctx.Entry(item).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            });
        }

        public virtual Task RemoveAsync(T item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            });
        }

        public virtual Task RemoveAllAsync()
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.Set<T>().RemoveRange(GetAllQuery(ctx.Set<T>()));
                await ctx.SaveChangesAsync();
            });
        }

        public virtual Task RemoveRangeAsync(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                CheckNull(item);
                CheckItem(item);
            }
            return ContextProvider.DoAsync(async ctx =>
            {
                var itemsEntities = ctx.Set<T>().Where(i => items.Contains(i));
                ctx.Set<T>().RemoveRange(itemsEntities);
                await ctx.SaveChangesAsync();
            });
        }

        public async Task<T[]> GetAllAsync()
        {
            T[] items = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                items = await GetIncludeQuery(GetSortQuery(GetAllQuery(ctx.Set<T>()))).AsNoTracking<T>().ToArrayAsync();
            });
            return items;
        }

        public Task<PageViewModel<T>> GetAllAsync(PageBindingModel pageBindingModel) =>
            PageTemplate<T>(pageBindingModel, e => GetIncludeQuery(GetSortQuery(GetAllQuery(e))));

        protected async Task<PageViewModel<TL>> PageTemplate<TL>(PageBindingModel bindingModel, Func<IQueryable<TL>, IQueryable<TL>> filters)
            where TL : class
        {
            PageViewModel<TL> pageViewModel = new PageViewModel<TL>();
            await ContextProvider.DoAsync(async ctx =>
            {
                pageViewModel.Items = await filters(ctx.Set<TL>()).Skip(bindingModel.ItemsOnPage * bindingModel.PageNumber).Take(bindingModel.ItemsOnPage).AsNoTracking<TL>().ToArrayAsync();
                pageViewModel.TotalCount = await filters(ctx.Set<TL>()).CountAsync();
            });
            return pageViewModel;
        }

        public async Task<T> GetById(int id)
        {
            T item = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                item = await GetIncludeQuery(GetAllQuery(ctx.Set<T>())).AsNoTracking<T>().FirstOrDefaultAsync(i => i.Id == id);
            });
            return item;
        }

        public async Task<T> GetByEntity(T e)
        {
            T item = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                item = await GetIncludeQuery(GetAllQuery(ctx.Set<T>())).AsNoTracking<T>().FirstOrDefaultAsync(i => i.Id == e.Id);
            });
            return item;
        }

        protected void CheckNull(T item)
        {
            if (item == null)
                throw new NullReferenceException(nameof(item));
        }

        protected virtual IQueryable<T> GetAllQuery(IQueryable<T> items) => items;

        protected virtual IQueryable<T> GetSortQuery(IQueryable<T> items) => items;

        protected virtual IQueryable<T> GetIncludeQuery(IQueryable<T> items) => items;

        protected virtual void CheckItem(T item) { }

        protected virtual void PrepareToAdd(T item) { }
    }
}