using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class TimeReadRepository : Repository<TimeRead>
    {
        public TimeReadRepository(Player player)
        {
            Player = player;
        }

        protected Player Player { get; }

        protected override IQueryable<TimeRead> GetAllQuery(IQueryable<TimeRead> items) =>
            items.Where(i => i.PlayerId == Player.Id);

        protected override IQueryable<TimeRead> GetSortQuery(IQueryable<TimeRead> items) =>
            items.OrderBy(i => i.Time);

        protected override void CheckItem(TimeRead item)
        {
            if(item.PlayerId != Player.Id) 
                throw new ArgumentException("Wrong PlayerId");
        }

        protected override void PrepareToAdd(TimeRead item)
        {
            item.PlayerId = Player.Id;
            item.Player = null;
        }
    }
}