using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public abstract class RepositoryTestsBase<T>
        where T: class, IEntityId
    {
        protected abstract T[] InitialItems { get; }

        protected abstract IRepository<T> Repository { get; }

        protected abstract bool TheSameData(T entity1, T entity2);

        protected bool TheSameDataAndId(T entity1, T entity2)
        {
            if (entity1.Id != entity2.Id) return false;
            return TheSameData(entity1, entity2);
        }

        protected bool TheSameDataArrays(T[] arrEntity1, T[] arrEntity2, Func<T, T, bool> compare)
        {
            if (arrEntity1.Length != arrEntity2.Length) return false;

            if (!Array.TrueForAll(arrEntity1, e1 =>
            {
                foreach (T e2 in arrEntity2)
                {
                    if (compare(e1, e2)) return true;
                }
                return false;
            }))
                return false;

            if (!Array.TrueForAll(arrEntity2, e2 =>
            {
                foreach (T e1 in arrEntity1)
                {
                    if (compare(e1, e2)) return true;
                }
                return false;
            }))
                return false;

            return true;

        }

        [SetUp]
        public async Task DataSetUp()
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Set<T>().RemoveRange(ctx.Set<T>());
                ctx.Set<T>().AddRange(InitialItems);
                await ctx.SaveChangesAsync();
            });
        }

        [Test]
        public async Task SetUpTest()
        {
            T[] loadedEntities = null;
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                loadedEntities = await ctx.Set<T>().ToArrayAsync();
            });
            Assert.IsTrue(TheSameDataArrays(loadedEntities, InitialItems, TheSameData));
        }

        [Test]
        public async Task AddAsyncTest()
        {
            await Repository.AddAsync(InitialItems[0]);

            T[] loadedEntities = null;

            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                loadedEntities = await ctx.Set<T>().ToArrayAsync();
            });

            T[] expectedEntities = InitialItems.Concat(new List<T> {InitialItems[0]}).ToArray();

            Assert.IsTrue(TheSameDataArrays(expectedEntities, loadedEntities, TheSameData));
        }

        [Test]
        public async Task AddRangeAsyncTest()
        {
            await Repository.AddRangeAsync(InitialItems);

            T[] loadedEntities = null;

            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                loadedEntities = await ctx.Set<T>().ToArrayAsync();
            });

            T[] expectedEntities = InitialItems.Concat(new List<T>(InitialItems)).ToArray();

            Assert.IsTrue(TheSameDataArrays(expectedEntities, loadedEntities, TheSameData));
        }

        [Test]
        public async Task UpdateAsyncTest()
        {
            T firstInDB = null;
            await NTimeDBContext.ContextDoAsync( async ctx =>
            {
                firstInDB = await ctx.Set<T>().FirstOrDefaultAsync();
            });

            T updatedItem = InitialItems[1];

            Assert.IsFalse(TheSameData(updatedItem, firstInDB));

            updatedItem.Id = firstInDB.Id;

            await Repository.UpdateAsync(updatedItem);

            T loadedEntiti = null;

            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                loadedEntiti = await ctx.Set<T>().FirstOrDefaultAsync(e => e.Id == updatedItem.Id);
            });

            Assert.IsTrue(TheSameDataAndId(loadedEntiti, updatedItem));
        }

        [Test]
        public async Task RemoveAsyncTest()
        {
            T firstInDB = null;
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                firstInDB = await ctx.Set<T>().FirstOrDefaultAsync();
            });

            await Repository.RemoveAsync(firstInDB);

            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                Assert.AreEqual(await ctx.Set<T>().FirstOrDefaultAsync(e => e.Id == firstInDB.Id), default(T));
            });
        }

        [TearDown]
        public async Task DataTearDown()
        {
            await NTimeDBContext.ContextDoAsync( async ctx =>
            {
                ctx.Set<T>().RemoveRange(ctx.Set<T>());
                await ctx.SaveChangesAsync();
            });
        }
    }
}