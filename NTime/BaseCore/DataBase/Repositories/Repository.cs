using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public abstract class Repository<T>
        where T : class, IEntityId
    { 
        public async Task<T> AddAsync(T item)
        {
            CheckNull(item);
            PrepareToAdd(item);
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Set<T>().Add(item);
                await ctx.SaveChangesAsync();
            });
            return item;
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                CheckNull(item);
                PrepareToAdd(item);
            }

            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                foreach (T item in items)
                    ctx.Entry(item).State = EntityState.Added;
                await ctx.SaveChangesAsync();
            });
        }

        public async Task UpdateAsync(T item)
        {
            CheckNull(item);
            CheckItem(item);
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            });
        }

        public async Task RemoveAsync(T item)
        {
            CheckNull(item);
            CheckItem(item);
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            });
        }

        public async Task RemoveAllAsync()
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Set<T>().RemoveRange(GetAllQuery(ctx.Set<T>()));
                await ctx.SaveChangesAsync();
            });
        }

        public async Task<T[]> GetAllAsync()
        {
            T[] items = null;
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                items = await GetSortQuery(GetAllQuery(ctx.Set<T>())).ToArrayAsync();
            });
            return items;
        }

        private void CheckNull(T item)
        {
            if(item == null)
                throw new NullReferenceException(nameof(item));
        }

        protected virtual IQueryable<T> GetAllQuery(IQueryable<T> items) => items;

        protected virtual IQueryable<T> GetSortQuery(IQueryable<T> items) => items;

        protected virtual void CheckItem(T item) { }

        protected virtual void PrepareToAdd(T item) { }
    }
}