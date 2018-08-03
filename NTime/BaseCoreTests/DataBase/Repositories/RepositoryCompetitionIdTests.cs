using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public abstract class RepositoryCompetitionIdTests<T> : RepositoryTests<T>
        where T: class, IEntityId, ICompetitionId
    {
        protected virtual Competition InitialCompetition { get; set; } = new Competition("Competition", DateTime.Now, null, null, null, null);

        protected override async Task BeforeDataSetUp(NTimeDBContext ctx)
        {
            InitialCompetition.AgeCategories = null; 
            InitialCompetition.Distances = null;
            InitialCompetition.Subcategories = null;
            InitialCompetition.Players = null;
            ctx.Competitions.Add(InitialCompetition);
            await ctx.SaveChangesAsync();
        }

        protected override Task AfterDataTearDown(NTimeDBContext ctx)
        {
            return Task.Factory.StartNew(() => ctx.Competitions.RemoveRange(ctx.Competitions));
        }

        protected override void Reset(T item)
        {
            item.Competition = null;
            item.CompetitionId = InitialCompetition.Id;
        }
    }
}