using System;
using System.Linq;

namespace BaseCore.DataBase
{
    public class LogsSourceRepository : Repository<LogsSource>
    {
        protected Gate Gate { get; }

        public LogsSourceRepository(IContextProvider contextProvider, Gate gate) : base(contextProvider)
        {
            Gate = gate;
        }

        protected override IQueryable<LogsSource> GetAllQuery(IQueryable<LogsSource> items) =>
            items.Where(i => i.GateId == Gate.Id);

        protected override IQueryable<LogsSource> GetSortQuery(IQueryable<LogsSource> items) =>
            items.OrderBy(i => i.GateId);

        protected override void CheckItem(LogsSource item)
        {
            if(item.GateId == Gate.Id) throw new ArgumentException("Wrong GateId");
        }

        protected override void PrepareToAdd(LogsSource item)
        {
            item.GateId = Gate.Id;
            item.Gate = null;
        }
    }
}