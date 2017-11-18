using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class GateOrderItemRepository : Repository<GatesOrderItem>
    {
        public GateOrderItemRepository(IContextProvider contextProvider, Distance distance) : base(contextProvider) => Distance = distance;

        protected Distance Distance { get; }

        protected override IQueryable<GatesOrderItem> GetAllQuery(IQueryable<GatesOrderItem> items) =>
            items.Where(i => i.DistanceId == Distance.Id);

        protected override IQueryable<GatesOrderItem> GetSortQuery(IQueryable<GatesOrderItem> items) =>
            items.OrderBy(i => i.OrderNumber);

        protected override IQueryable<GatesOrderItem> GetIncludeQuery(IQueryable<GatesOrderItem> items) =>
            items.Include(i => i.Gate);

        protected override void CheckItem(GatesOrderItem item)
        {
            if(item.DistanceId != Distance.Id) 
                throw new ArgumentException("Wrong DistanceId");
        }

        protected override void PrepareToAdd(GatesOrderItem item)
        {
            item.DistanceId = Distance.Id;
            item.Distance = null;
        }

        public async Task ReplaceBy(IEnumerable<GatesOrderItem> gatesOrder)
        {
            int c = 0;
            foreach (GatesOrderItem item in gatesOrder)
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