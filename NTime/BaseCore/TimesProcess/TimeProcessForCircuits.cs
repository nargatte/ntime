using System.Collections.Generic;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForCircuits : TimeProcessForDistance
    {
        public TimeProcessForCircuits(Player player, Distance distance, GatesOrderItem[] gateOrderItem, HashSet<int> gatesNumbers, TimeProcess timeProcess) : base(player, distance, gateOrderItem, gatesNumbers, timeProcess)
        {
        }

        protected override IEnumerable<GatesOrderItem> GatesOrderNumbers()
        {
            yield return GateOrderItem[0];
            while (Distance.LapsCount > Laps)
            {
                for (int i = 1; i < GateOrderItem.Length; i++)
                {
                    yield return GateOrderItem[i];
                }
                Laps++;
            }
        }
    }
}