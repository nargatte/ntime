using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [SetUpFixture]
    public class ClearDataBase
    {
        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            NTimeDBContext.ContextDo(ctx =>
            {
                ctx.Competitions.RemoveRange(ctx.Competitions);
                ctx.CompetitionTypes.RemoveRange(ctx.CompetitionTypes);
                ctx.AgeCategories.RemoveRange(ctx.AgeCategories);
                ctx.AgeCategoryCollections.RemoveRange(ctx.AgeCategoryCollections);
                ctx.AgeCategoryTemplates.RemoveRange(ctx.AgeCategoryTemplates);
                ctx.Distances.RemoveRange(ctx.Distances);
                ctx.ExtraPlayerInfo.RemoveRange(ctx.ExtraPlayerInfo);
                ctx.Players.RemoveRange(ctx.Players);
                ctx.ReaderOrders.RemoveRange(ctx.ReaderOrders);
                ctx.TimeReads.RemoveRange(ctx.TimeReads);
            });
        }
    }
}