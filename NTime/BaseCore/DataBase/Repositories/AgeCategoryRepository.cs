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

        public async Task<AgeCategory> GetFittingAsync(Player player)
        {
            AgeCategory ret = null;
            await ContextProvider.DoAsync(async ctx =>
            {
                ret = await ctx.AgeCategories.AsNoTracking().FirstOrDefaultAsync(i => i.YearFrom <= player.BirthDate.Year && player.BirthDate.Year <= i.YearTo);
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
                    .Select(e => new AgeCategory(e.Name, e.YearFrom, e.YearTo) { CompetitionId = Competition.Id })
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
                                .Select(a => new AgeCategoryTemplate(a.Name, a.YearFrom, a.YearTo)
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


    }
}