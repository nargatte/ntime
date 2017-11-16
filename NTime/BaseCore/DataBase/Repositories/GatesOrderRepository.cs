using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class GatesOrderRepository : Repository<GatesOrder>
    {
        public GatesOrderRepository(IContextProvider contextProvider, Distance distance) : base(contextProvider) => Distance = distance;

        protected Distance Distance { get; }

        protected override IQueryable<GatesOrder> GetAllQuery(IQueryable<GatesOrder> items) =>
            items.Where(i => i.DistanceId == Distance.Id);

        protected override IQueryable<GatesOrder> GetSortQuery(IQueryable<GatesOrder> items) =>
            items.OrderBy(i => i.OrderNumber);

        protected override void CheckItem(GatesOrder item)
        {
            if(item.DistanceId != Distance.Id) 
                throw new ArgumentException("Wrong DistanceId");
        }

        protected override void PrepareToAdd(GatesOrder item)
        {
            item.DistanceId = Distance.Id;
            item.Distance = null;
        }

        public async Task ReplaceBy(IEnumerable<GatesOrder> gatesOrder)
        {
            int c = 0;
            foreach (GatesOrder item in gatesOrder)
            {
                CheckNull(item);
                PrepareToAdd(item);
                item.OrderNumber = c;
                c++;
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.GatesOrders.RemoveRange(GetAllQuery(ctx.GatesOrders));
                ctx.GatesOrders.AddRange(gatesOrder);
                await ctx.SaveChangesAsync();
            });
        }
    }
}