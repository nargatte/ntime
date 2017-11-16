using System.Collections.Generic;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForCircuits : TimeProcessForDistance
    {
        public TimeProcessForCircuits(Player player, Distance distance, GatesOrder[] gatesOrder, HashSet<int> gatesNumbers, TimeProcess timeProcess) : base(player, distance, gatesOrder, gatesNumbers, timeProcess)
        {
        }

        protected override IEnumerable<GatesOrder> GatesOrderNumers()
        {
            yield return GatesOrder[0];
            while (Distance.LapsCount > Laps)
            {
                for (int i = 1; i < GatesOrder.Length; i++)
                {
                    yield return GatesOrder[i];
                }
                Laps++;
            }
        }
    }
}