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
            yield return ReaderOrder[0];
            while (true)
            {
                for (int i = 1; i < ReaderOrder.Length; i++)
                {
                    yield return ReaderOrder[i];
                }
                Laps++;
            }
        }

        protected override bool IsNonsignificantAfter(TimeRead timeRead)
        {
            if (_afterCompetition) return true;
            _afterCompetition = timeRead.Time - StartTime >= Distance.TimeLimit && ExpectedReader.Current == ReaderOrder[1];
            return _afterCompetition;
        }

        protected override bool IsRankabe() => _afterCompetition;
    }
}