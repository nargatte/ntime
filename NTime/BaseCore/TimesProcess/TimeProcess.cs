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

        internal decimal MinRepetSeconds;

        public TimeProcess(Competition competition, decimal minRepetSeconds = 60)
        {
            Competition = competition;
            MinRepetSeconds = minRepetSeconds;
        }

        public async Task ProcessAllAsync()
        {
            Task<Player[]> tp = new PlayerRepository(new ContextProvider(), Competition).GetAllAsync();
            Task<Distance[]> td = new DistanceRepository(new ContextProvider(), Competition).GetAllAsync();
            await Task.WhenAll(tp, td);

            Dictionary<int, Distance> distancesDictionary = new Dictionary<int, Distance>();
            Dictionary<int, GatesOrder[]> readerOrdersesDictionary = new Dictionary<int, GatesOrder[]>();
            Dictionary<int, HashSet<int>> readersNumbersDictionary = new Dictionary<int, HashSet<int>>();

            foreach (Distance distance in td.Result)
            {
                distancesDictionary.Add(distance.Id, distance);
                GatesOrder[] readerOrders = await GetReaderOrder(distance);
                readerOrdersesDictionary.Add(distance.Id, readerOrders);
                readersNumbersDictionary.Add(distance.Id, GetReadersNumbers(readerOrders));
            }

            List<Task> tasks = new List<Task>(tp.Result.Where(p => p.DistanceId != null)
                .Select(p => ProcessForPlayer(p, distancesDictionary[p.DistanceId.Value], readerOrdersesDictionary[p.DistanceId.Value], readersNumbersDictionary[p.DistanceId.Value])));

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

            GatesOrder[] readerOrders = await GetReaderOrder(distance);
            await ProcessForPlayer(player, distance, readerOrders, GetReadersNumbers(readerOrders));
        }

        public Task ProcessForPlayer(Player player, Distance distance, GatesOrder[] readerOrder, HashSet<int> readersNumbers)
        {
            TimeProcessForDistance timeProcess = null;
            if (distance.DistanceTypeEnum == DistanceTypeEnum.DeterminedDistance)
                timeProcess = new TimeProcessForDistance(player, distance, readerOrder, readersNumbers, this);
            if (distance.DistanceTypeEnum == DistanceTypeEnum.DeterminedLaps)
                timeProcess = new TimeProcessForCircuits(player, distance, readerOrder, readersNumbers, this);
            if (distance.DistanceTypeEnum == DistanceTypeEnum.LimitedTime)
                timeProcess = new TimeProcessForTime(player, distance, readerOrder, readersNumbers, this);

            if (timeProcess != null)
                return timeProcess.FullProcess();
            return Task.Run(() => { });
        }

        private Task<GatesOrder[]> GetReaderOrder(Distance distance) => new GatesOrderRepository(new ContextProvider(), distance).GetAllAsync();

        private HashSet<int> GetReadersNumbers(GatesOrder[] readerOrders) => new HashSet<int>(readerOrders.Select(r => r.GateNumber));
    }
}