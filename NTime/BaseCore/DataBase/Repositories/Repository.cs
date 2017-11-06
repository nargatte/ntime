using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class Repository<T> : IRepository<T>
        where T : class, IEntityId
    { 
        public async Task<T> AddAsync(T item)
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Added;
                await ctx.SaveChangesAsync();
            });
            return item;
        }

        public async Task AddRangeAsync(IEnumerable<T> items)
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                foreach (T item in items)
                    ctx.Entry(item).State = EntityState.Added;
                await ctx.SaveChangesAsync();
            });
        }

        public async Task UpdateAsync(T item)
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            });
        }

        public async Task RemoveAsync(T item)
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Entry(item).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            });
        }
    }
}