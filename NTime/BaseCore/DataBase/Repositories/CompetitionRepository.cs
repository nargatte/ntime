using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class CompetitionRepository : Repository<Competition>
    {
        public CompetitionRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        protected override IQueryable<Competition> GetSortQuery(IQueryable<Competition> items) =>
            items.OrderByDescending(e => e.EventDate);

        public async Task<Competition> AddWithAgeCategoryCollection(Competition competition, AgeCategoryCollection ageCategoryCollection )
        {
            await ContextProvider.DoAsync(async ctx =>
            {
                using (DbContextTransaction contextTransaction = ctx.Database.BeginTransaction())
                {
                    try
                    {
                        ctx.Competitions.Add(competition);
                        await ctx.SaveChangesAsync();

                        AgeCategoryTemplate[] ageCategoryTemplates =
                            await ctx.AgeCategoryTemplates
                                .Where(e => e.AgeCategoryCollectionId == ageCategoryCollection.Id).ToArrayAsync();
                        AgeCategory[] ageCategories = ageCategoryTemplates
                            .Select(e => new AgeCategory(e.Name, e.YearFrom, e.YearTo) {CompetitionId = competition.Id})
                            .ToArray();
                        ctx.AgeCategories.AddRange(ageCategories);
                        await ctx.SaveChangesAsync();

                        contextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        contextTransaction.Rollback();
                    }
                }
            });
            return competition;
        }
    }
}