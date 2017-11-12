using System.Collections.Generic;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForTime : TimeProcessForDistance
    {
        public TimeProcessForTime(Player player, Distance distance, ReaderOrder[] readerOrder, HashSet<int> readersNumbers, TimeProcess timeProcess) : base(player, distance, readerOrder, readersNumbers, timeProcess)
        {
        }

        private bool _afterCompetition;

        protected override IEnumerable<ReaderOrder> ReaderOrderNumers()
        {
            while (true)
            {
                foreach (ReaderOrder readerOrder in ReaderOrder)
                {
                    yield return readerOrder;
                }
                Circuits++;
            }
        }

        protected override bool IsNonsignificantAfter(TimeRead timeRead)
        {
            if (_afterCompetition) return true;
            _afterCompetition = timeRead.Time - StartTime < Distance.TimeLimit && ExpectedReader.Current == ReaderOrder[0];
            return _afterCompetition;
        }
    }
}