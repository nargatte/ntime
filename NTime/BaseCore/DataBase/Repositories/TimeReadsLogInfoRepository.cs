using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.Csv;

namespace BaseCore.DataBase
{
    public class TimeReadsLogInfoRepository : Repository<TimeReadsLogInfo>
    {
        protected Gate Gate { get; }

        public TimeReadsLogInfoRepository(IContextProvider contextProvider, Gate gate) : base(contextProvider)
        {
            Gate = gate;
        }

        protected override IQueryable<TimeReadsLogInfo> GetAllQuery(IQueryable<TimeReadsLogInfo> items) =>
            items.Where(i => i.GateId == Gate.Id);

        protected override IQueryable<TimeReadsLogInfo> GetSortQuery(IQueryable<TimeReadsLogInfo> items) =>
            items.OrderBy(i => i.GateId);

        protected override void CheckItem(TimeReadsLogInfo item)
        {
            if (item.GateId != Gate.Id) throw new ArgumentException("Wrong GateId");
        }

        protected override void PrepareToAdd(TimeReadsLogInfo item)
        {
            item.GateId = Gate.Id;
            item.Gate = null;
        }

    }
}