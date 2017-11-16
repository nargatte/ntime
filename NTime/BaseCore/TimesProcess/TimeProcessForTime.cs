using System.Collections.Generic;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForTime : TimeProcessForDistance
    {
        public TimeProcessForTime(Player player, Distance distance, GatesOrder[] gatesOrder, HashSet<int> readersNumbers, TimeProcess timeProcess) : base(player, distance, gatesOrder, readersNumbers, timeProcess)
        {
        }

        private bool _afterCompetition;

        protected override IEnumerable<GatesOrder> GatesOrderNumers()
        {
            yield return GatesOrder[0];
            while (true)
            {
                for (int i = 1; i < GatesOrder.Length; i++)
                {
                    yield return GatesOrder[i];
                }
                Laps++;
            }
        }

        protected override bool IsNonsignificantAfter(TimeRead timeRead)
        {
            if (_afterCompetition) return true;
            _afterCompetition = timeRead.Time - StartTime >= Distance.TimeLimit && ExpectedReader.Current == GatesOrder[1];
            return _afterCompetition;
        }

        protected override bool IsRankabe() => _afterCompetition;
    }
}