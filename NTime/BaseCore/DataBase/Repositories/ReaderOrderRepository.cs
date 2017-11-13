using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ReaderOrderRepository : Repository<ReaderOrder>
    {
        public ReaderOrderRepository(IContextProvider contextProvider, Distance distance) : base(contextProvider) => Distance = distance;

        protected Distance Distance { get; }

        protected override IQueryable<ReaderOrder> GetAllQuery(IQueryable<ReaderOrder> items) =>
            items.Where(i => i.DistanceId == Distance.Id);

        protected override IQueryable<ReaderOrder> GetSortQuery(IQueryable<ReaderOrder> items) =>
            items.OrderBy(i => i.OrderNumber);

        protected override void CheckItem(ReaderOrder item)
        {
            if(item.DistanceId != Distance.Id) 
                throw new ArgumentException("Wrong DistanceId");
        }

        protected override void PrepareToAdd(ReaderOrder item)
        {
            item.DistanceId = Distance.Id;
            item.Distance = null;
        }

        public async Task ReplaceBy(IEnumerable<ReaderOrder> readersOrder)
        {
            int c = 0;
            foreach (ReaderOrder item in readersOrder)
            {
                CheckNull(item);
                PrepareToAdd(item);
                item.OrderNumber = c;
                c++;
            }

            await ContextProvider.DoAsync(async ctx =>
            {
                ctx.ReaderOrders.RemoveRange(GetAllQuery(ctx.ReaderOrders));
                ctx.ReaderOrders.AddRange(readersOrder);
                await ctx.SaveChangesAsync();
            });
        }
    }
}