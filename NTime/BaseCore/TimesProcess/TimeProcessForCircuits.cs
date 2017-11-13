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
            for (int i = 0; i < Distance.LapsCount; i++)
            {
                foreach (ReaderOrder readerOrder in ReaderOrder)
                {
                    yield return readerOrder;
                }
                Circuits++;
            }
        }
    }
}