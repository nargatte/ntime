using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [TestFixture]
    public abstract class RepositoryTests<T>
        where T: class, IEntityId
    {
        protected abstract T[] InitialItems { get; set; }

        protected abstract Repository<T> Repository { get; }

        protected abstract bool TheSameData(T entity1, T entity2);

        protected virtual bool SortTester(T before, T after) => true;

        protected virtual Task BeforeDataSetUp(NTimeDBContext context) => Task.Factory.StartNew(() => { });

        protected virtual Task AfterDataTearDown(NTimeDBContext context) => Task.Factory.StartNew(() => { });

        protected virtual void Reset(T item) { }

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
        public virtual async Task DataSetUp()
        {
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                ctx.Set<T>().RemoveRange(ctx.Set<T>());
                await AfterDataTearDown(ctx);
                await ctx.SaveChangesAsync();
            });

            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                await BeforeDataSetUp(ctx);
                Array.ForEach(InitialItems, Reset);
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

            if (TheSameData(updatedItem, firstInDB))
            {
                await NTimeDBContext.ContextDoAsync(async ctx =>
                {
                    firstInDB = await ctx.Set<T>().FirstOrDefaultAsync(e => e.Id != firstInDB.Id);
                });
            }

            Reset(updatedItem);

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

        [Test]
        public async Task RemoveAllAsyncTest()
        {
            await Repository.RemoveAllAsync();
            await NTimeDBContext.ContextDoAsync(async ctx =>
            {
                Assert.Zero(await ctx.Set<T>().CountAsync());
            });
        }

        [Test]
        public async Task GetAllAsyncTest()
        {
            T[] items = await Repository.GetAllAsync();
            Assert.IsTrue(TheSameDataArrays(items, InitialItems, TheSameData));
        }


        [Test]
        public async Task GetAllAsyncTestSort()
        {
            T[] items = await Repository.GetAllAsync();
            for (int i = 1; i < items.Length; i++)
                Assert.IsTrue(SortTester(items[i-1], items[i]));
        }
    }
}