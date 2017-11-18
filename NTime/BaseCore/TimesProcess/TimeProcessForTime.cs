using System.Collections.Generic;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForTime : TimeProcessForDistance
    {
        public TimeProcessForTime(Player player, Distance distance, GatesOrderItem[] gateOrderItem, HashSet<int> readersNumbers, TimeProcess timeProcess) : base(player, distance, gateOrderItem, readersNumbers, timeProcess)
        {
        }

        private bool _afterCompetition;

        protected override IEnumerable<GatesOrderItem> GatesOrderNumers()
        {
            yield return GateOrderItem[0];
            while (true)
            {
                for (int i = 1; i < GateOrderItem.Length; i++)
                {
                    yield return GateOrderItem[i];
                }
                Laps++;
            }
        }

        protected override bool IsNonsignificantAfter(TimeRead timeRead)
        {
            if (_afterCompetition) return true;
            _afterCompetition = timeRead.Time - StartTime >= Distance.TimeLimit && ExpectedReader.Current == GateOrderItem[1];
            return _afterCompetition;
        }

        protected override bool IsRankabe() => _afterCompetition;
    }
}