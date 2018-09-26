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

        public Task<GatesOrderItem> AddAsync(GatesOrderItem gatesOrderItem, Gate gate)
        {
            gatesOrderItem.GateId = gate.Id;
            gatesOrderItem.Gate = null;

            return AddAsync(gatesOrderItem);
        }

        public Task UpdateAsync(GatesOrderItem gatesOrderItem, Gate gate)
        {
            gatesOrderItem.GateId = gate.Id;
            gatesOrderItem.Gate = null;

            return UpdateAsync(gatesOrderItem);
        }

        public async Task ReplaceByAsync(IEnumerable<Tuple<GatesOrderItem, Gate>> gatesOrder)
        {
            int c = 0;
            foreach (Tuple<GatesOrderItem, Gate> item in gatesOrder)
            {
                CheckNull(item.Item1);
                PrepareToAdd(item.Item1);

                item.Item1.GateId = item.Item2?.Id;
                item.Item1.Gate = null;

                item.Item1.OrderNumber = c;
                c++;
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.GatesOrderItems.RemoveRange(GetAllQuery(ctx.GatesOrderItems));
                ctx.GatesOrderItems.AddRange(gatesOrder.Select(so => so.Item1));
                await ctx.SaveChangesAsync();
            });
        }
    }
}