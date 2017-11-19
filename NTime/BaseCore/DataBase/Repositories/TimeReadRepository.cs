using System;
using System.Data.Entity;
using System.Linq;

namespace BaseCore.DataBase
{
    public class TimeReadRepository : Repository<TimeRead>
    {
        public TimeReadRepository(IContextProvider contextProvider, Player player) : base(contextProvider) => Player = player;

        private Player Player { get; }
        protected override IQueryable<TimeRead> GetAllQuery(IQueryable<TimeRead> items) =>
            items.Where(i => i.PlayerId == Player.Id);

        protected override IQueryable<TimeRead> GetSortQuery(IQueryable<TimeRead> items) =>
            items.OrderBy(i => i.Time);

        protected override void CheckItem(TimeRead item)
        {
            if(item.PlayerId != Player.Id) 
                throw new ArgumentException("Wrong PlayerId");

            item.Gate = null;
        }

        protected override void PrepareToAdd(TimeRead item)
        {
            item.PlayerId = Player.Id;
            item.Player = null;

            item.Gate = null;
        }

        protected override IQueryable<TimeRead> GetIncludeQuery(IQueryable<TimeRead> items) =>
        items.Include(i => i.Gate);
    }
}