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
    public abstract class RepositoryCompetitionIdTests<T> : RepositoryTests<T>
        where T: class, IEntityId, ICompetitionId
    {
        protected virtual Competition InitialCompetition { get; set; } = new Competition("Competition", DateTime.Now, null, null, null, null, CompetitionTypeEnum.DeterminedDistance);

        protected override async Task BeforeDataSetUp(NTimeDBContext ctx)
        {
            ctx.Competitions.Add(InitialCompetition);
            await ctx.SaveChangesAsync();
            Array.ForEach(InitialItems, i => i.CompetitionId = InitialCompetition.Id);
        }

        protected override Task AfterDataTearDown(NTimeDBContext ctx)
        {
            return Task.Factory.StartNew(() => ctx.Competitions.RemoveRange(ctx.Competitions));
        }
    }
}