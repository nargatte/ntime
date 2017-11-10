using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class TimeReadRepository : Repository<TimeRead>
    {
        private Player _player;

        public TimeReadRepository(IContextProvider contextProvider) : base(contextProvider)
        {
        }

        public Player Player
        {
            get
            {
                if (_player == null)
                    throw new NullReferenceException("Player is unset");
                return _player;
            }
            set => _player = value;
        }

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