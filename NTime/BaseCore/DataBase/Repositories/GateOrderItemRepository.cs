using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class GateOrderItemRepository : Repository<GateOrderItem>
    {
        public GateOrderItemRepository(IContextProvider contextProvider, Distance distance) : base(contextProvider) => Distance = distance;

        protected Distance Distance { get; }

        protected override IQueryable<GateOrderItem> GetAllQuery(IQueryable<GateOrderItem> items) =>
            items.Where(i => i.DistanceId == Distance.Id);

        protected override IQueryable<GateOrderItem> GetSortQuery(IQueryable<GateOrderItem> items) =>
            items.OrderBy(i => i.OrderNumber);

        protected override IQueryable<GateOrderItem> GetIncludeQuery(IQueryable<GateOrderItem> items) =>
            items.Include(i => i.Gate);

        protected override void CheckItem(GateOrderItem item)
        {
            if(item.DistanceId != Distance.Id) 
                throw new ArgumentException("Wrong DistanceId");
        }

        protected override void PrepareToAdd(GateOrderItem item)
        {
            item.DistanceId = Distance.Id;
            item.Distance = null;
        }

        public async Task ReplaceBy(IEnumerable<GateOrderItem> gatesOrder)
        {
            int c = 0;
            foreach (GateOrderItem item in gatesOrder)
            {
                CheckNull(item);
                PrepareToAdd(item);
                item.OrderNumber = c;
                c++;
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.GatesOrder.RemoveRange(GetAllQuery(ctx.GatesOrder));
                ctx.GatesOrder.AddRange(gatesOrder);
                await ctx.SaveChangesAsync();
            });
        }
    }
}