using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    public class TimeProcess
    {
        internal Competition Competition;

        internal decimal MinRepeatSeconds;

        public TimeProcess(Competition competition, decimal minRepeatSeconds = 60)
        {
            Competition = competition;
            MinRepeatSeconds = minRepeatSeconds;
        }

        public async Task ProcessAllAsync()
        {
            Task<Player[]> tp = new PlayerRepository(new ContextProvider(), Competition).GetAllAsync();
            Task<Distance[]> td = new DistanceRepository(new ContextProvider(), Competition).GetAllAsync();
            await Task.WhenAll(tp, td);

            Dictionary<int, Distance> distancesDictionary = new Dictionary<int, Distance>();
            Dictionary<int, GatesOrderItem[]> readersOrderDictionary = new Dictionary<int, GatesOrderItem[]>();
            Dictionary<int, HashSet<int>> readersNumbersDictionary = new Dictionary<int, HashSet<int>>();

            foreach (Distance distance in td.Result)
            {
                distancesDictionary.Add(distance.Id, distance);
                GatesOrderItem[] readerOrderItems = await GetReadersOrder(distance);
                readersOrderDictionary.Add(distance.Id, readerOrderItems);
                readersNumbersDictionary.Add(distance.Id, GetReadersNumbers(readerOrderItems));
            }

            List<Task> tasks = new List<Task>(tp.Result.Where(p => p.DistanceId != null)
                .Select(p => ProcessForPlayer(p, distancesDictionary[p.DistanceId.Value], readersOrderDictionary[p.DistanceId.Value], readersNumbersDictionary[p.DistanceId.Value])));

            await Task.WhenAll(tasks.ToArray());
        }

        public async Task ProcessSingleAsync(Player player, decimal minRepetSeconds = 60)
        {
            Distance distance;
            if (player.DistanceId != null)
            {
                distance =
                    await (new DistanceRepository(new ContextProvider(), Competition)).GetById(player.DistanceId.Value);
            }
            else
            {
                throw new ArgumentException("player.DistanceId == null");
            }

            GatesOrderItem[] readerOrdersItem = await GetReadersOrder(distance);
            await ProcessForPlayer(player, distance, readerOrdersItem, GetReadersNumbers(readerOrdersItem));
        }

        public Task ProcessForPlayer(Player player, Distance distance, GatesOrderItem[] readerOrderItem, HashSet<int> readersNumbers)
        {
            TimeProcessForDistance timeProcess = null;
            switch (distance.DistanceTypeEnum)
            {
                case DistanceTypeEnum.DeterminedDistance:
                    timeProcess = new TimeProcessForDistance(player, distance, readerOrderItem, readersNumbers, this);
                    break;
                case DistanceTypeEnum.DeterminedLaps:
                    timeProcess = new TimeProcessForCircuits(player, distance, readerOrderItem, readersNumbers, this);
                    break;
                case DistanceTypeEnum.LimitedTime:
                    timeProcess = new TimeProcessForTime(player, distance, readerOrderItem, readersNumbers, this);
                    break;
            }
            if (distance.DistanceTypeEnum == DistanceTypeEnum.DeterminedDistance)
                timeProcess = new TimeProcessForDistance(player, distance, readerOrderItem, readersNumbers, this);
            if (distance.DistanceTypeEnum == DistanceTypeEnum.DeterminedLaps)
                timeProcess = new TimeProcessForCircuits(player, distance, readerOrderItem, readersNumbers, this);
            if (distance.DistanceTypeEnum == DistanceTypeEnum.LimitedTime)
                timeProcess = new TimeProcessForTime(player, distance, readerOrderItem, readersNumbers, this);

            if (timeProcess != null)
                return timeProcess.FullProcess();
            return Task.Run(() => { });
        }

        private Task<GatesOrderItem[]> GetReadersOrder(Distance distance) => new GateOrderItemRepository(new ContextProvider(), distance).GetAllAsync();

        private HashSet<int> GetReadersNumbers(GatesOrderItem[] readerOrdersItem) => new HashSet<int>(readerOrdersItem.Select(r => r.Gate.Number));
    }
}