using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraPlayerInfoRepository : RepositoryCompetitionId<ExtraPlayerInfo>
    {
        public ExtraPlayerInfoRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<ExtraPlayerInfo> GetSortQuery(IQueryable<ExtraPlayerInfo> items) =>
            items.OrderBy(e => e.Name);

        public override Task RemoveAsync(ExtraPlayerInfo item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.ExtraPlayerInfo.Attach(item);
                await ctx.Players.Where(p => p.ExtraPlayerInfoId == item.Id).ForEachAsync(p =>
                {
                    p.ExtraPlayerInfo = null;
                    p.ExtraPlayerInfoId = null;
                });
                ctx.ExtraPlayerInfo.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }
    }
}