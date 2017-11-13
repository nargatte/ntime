using System.Collections.Generic;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForCircuits : TimeProcessForDistance
    {
        public TimeProcessForCircuits(Player player, Distance distance, ReaderOrder[] readerOrder, HashSet<int> readersNumbers, TimeProcess timeProcess) : base(player, distance, readerOrder, readersNumbers, timeProcess)
        {
        }

        protected override IEnumerable<ReaderOrder> ReaderOrderNumers()
        {
            yield return ReaderOrder[0];
            while (Distance.LapsCount > Laps)
            {
                for (int i = 1; i < ReaderOrder.Length; i++)
                {
                    yield return ReaderOrder[i];
                }
                Laps++;
            }
        }
    }
}