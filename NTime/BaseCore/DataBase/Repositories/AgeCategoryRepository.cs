using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class AgeCategoryRepository : RepositoryCompetitionId<AgeCategory>
    {
        public AgeCategoryRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<AgeCategory> GetSortQuery(IQueryable<AgeCategory> items) =>
            items.OrderBy(e => e.YearFrom);

        public async Task<AgeCategory[]> GetFittingAsync(Player player)
        {
            AgeCategory[] ret = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                ret = await GetAllQuery(ctx.AgeCategories).AsNoTracking().Where(i => i.YearFrom <= player.BirthDate.Year && player.BirthDate.Year <= i.YearTo && player.IsMale == i.Male).ToArrayAsync();
            });
            return ret;
        }

        public Task AddFormCollection(AgeCategoryCollection ageCategoryCollection)
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                AgeCategoryTemplate[] ageCategoryTemplates =
                    await ctx.AgeCategoryTemplates
                        .Where(e => e.AgeCategoryCollectionId == ageCategoryCollection.Id).ToArrayAsync();
                AgeCategory[] ageCategories = ageCategoryTemplates
                    .Select(e => new AgeCategory(e.Name, e.YearFrom, e.YearTo, e.Male) { CompetitionId = Competition.Id })
                    .ToArray();
                ctx.AgeCategories.AddRange(ageCategories);
                await ctx.SaveChangesAsync();
            });
        }

        public Task SaveAsCollection(AgeCategoryCollection ageCategoryCollection)
        {
            return ContextProvider.DoAsync(async ctx =>
            {
                using (DbContextTransaction contextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        ageCategoryCollection.AgeCategoryTemplates = null;
                        ctx.AgeCategoryCollections.Add(ageCategoryCollection);
                        await ctx.SaveChangesAsync();

                        AgeCategory[] ageCategories = await GetAllQuery(ctx.AgeCategories).ToArrayAsync();
                        AgeCategoryTemplate[] ageCategoryTemplates =
                            ageCategories
                                .Select(a => new AgeCategoryTemplate(a.Name, a.YearFrom, a.YearTo, a.Male)
                                {
                                    AgeCategoryCollectionId = ageCategoryCollection.Id
                                }).ToArray();
                        ctx.AgeCategoryTemplates.AddRange(ageCategoryTemplates);
                        await ctx.SaveChangesAsync();

                        contextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        contextTransaction.Rollback();
                    }
                }
            });
        }

        public override Task RemoveAsync(AgeCategory item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.AgeCategories.Attach(item);
                await ctx.Players.Where(p => p.AgeCategoryId == item.Id).ForEachAsync(p =>
                {
                    p.AgeCategory = null;
                    p.AgeCategoryId = null;
                });
                ctx.AgeCategories.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }
        public async Task<bool> IsAgeCategoryAndDistanceMatch(AgeCategory ageCategory, Distance distance)
        {
            bool answer = false;
            await ContextProvider.DoAsync(async ctx =>
            {
                answer = await ctx.AgeCategoryDistances.AnyAsync(acd =>
                    acd.CompetitionId == Competition.Id && acd.AgeCategoryId == ageCategory.Id &&
                    acd.DistanceId == distance.Id);
            });
            return answer;
        }

    }
}