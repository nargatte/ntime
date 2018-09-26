using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class GateRepository : RepositoryCompetitionId<Gate>
    {
        public GateRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<Gate> GetSortQuery(IQueryable<Gate> items) =>
            items.OrderBy(i => i.Number);

        public override Task RemoveAsync(Gate item)
        {
            CheckNull(item);
            CheckItem(item);

            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.Gates.Attach(item);
                await ctx.GatesOrderItems.Where(goi => goi.GateId == item.Id).ForEachAsync(goi =>
                {
                    ctx.GatesOrderItems.Remove(goi);
                });

                ctx.Gates.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }
    }
}