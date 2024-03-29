﻿using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class DistanceRepository : RepositoryCompetitionId<Distance>
    {
        public DistanceRepository(IContextProvider contextProvider, Competition competition) : base(contextProvider, competition)
        {
        }

        protected override IQueryable<Distance> GetSortQuery(IQueryable<Distance> items) =>
            items.OrderBy(e => e.Name);

        public override Task RemoveAsync(Distance item)
        {
            CheckNull(item);
            CheckItem(item);
            return ContextProvider.DoAsync(async ctx =>
            {
                ctx.Distances.Attach(item);
                await ctx.Players.Where(p => p.DistanceId == item.Id).ForEachAsync(p =>
                {
                    p.Distance = null;
                    p.DistanceId = null;
                });

                await ctx.AgeCategoryDistances.Where(acd => acd.DistanceId == item.Id).ForEachAsync(acd =>
                {
                    ctx.AgeCategoryDistances.Remove(acd);
                });

                await ctx.GatesOrderItems.Where(go => go.DistanceId == item.Id).ForEachAsync(go =>
                {
                    ctx.GatesOrderItems.Remove(go);
                });
                ctx.Distances.Remove(item);
                await ctx.SaveChangesAsync();
            });
        }
    }
}