using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    internal class TimeProcessForDistance
    {
        protected Player Player;
        protected Distance Distance;
        protected GatesOrderItem[] GateOrderItem;
        protected HashSet<int> ReadersNumbers;
        protected TimeRead[] TimeReads;
        protected HashSet<TimeRead> ExistingVoids = new HashSet<TimeRead>();
        protected decimal StartTime;
        protected TimeProcessManager TimeProcess;
        protected int Laps;
        protected TimeRead LastSignificant; // Last correct Read. If null - no correct reads detected yet
        protected IEnumerator<GatesOrderItem> ExpectedReader;
        protected bool NoReadersRemaining;

        protected internal TimeProcessForDistance(Player player, Distance distance, GatesOrderItem[] readerOrderItem, HashSet<int> readersNumbers,  TimeProcessManager timeProcess)
        {
            Player = player;
            Distance = distance;
            GateOrderItem = readerOrderItem;
            TimeProcess = timeProcess;
            ReadersNumbers = readersNumbers;
        }

        public async Task FullProcess()
        {
            await Initialize();
            if (Player.StartTime == null)
                return;
            if (!Array.TrueForAll(TimeReads, t => ReaderNumberExist(t.Gate.Number)))
                return;

            ExpectedReader = GatesOrderNumbers().GetEnumerator();
            NoReadersRemaining = !ExpectedReader.MoveNext();

            int timeReadsIterator = 0;
            while (timeReadsIterator < TimeReads.Length)
            {
                if (LoopBody(TimeReads[timeReadsIterator]))
                    timeReadsIterator++;
            }

            if (LastSignificant == null)
                return;
            Task t1 = UpdateVoids();
            Task t2 = UpdatePlayer();
            Task t3 = UpdateTimes();

            await Task.WhenAll(t1, t2, t3);
        }

        private Task UpdateVoids()
        {
            TimeReadRepository readRepository = new TimeReadRepository(new ContextProvider(), Player);
            return readRepository.AddRangeAsync(ExistingVoids);

        }

        private Task UpdatePlayer()
        {
            PlayerRepository playerRepository = new PlayerRepository(new ContextProvider(), TimeProcess.Competition);
            Player.LapsCount = Laps;
            Player.Time = LastSignificant.Time - StartTime;
            Player.CompetitionCompleted = IsRankable();
            return playerRepository.UpdateAsync(Player);
        }

        private Task UpdateTimes()
        {
            TimeReadRepository timeReadRepository = new TimeReadRepository(new ContextProvider(), Player);
            return timeReadRepository.UpdateRangeAsync(TimeReads);
        }

        private async Task LoadTimeReads()
        {
            TimeReads = await new TimeReadRepository(new ContextProvider(), Player).GetAllAsync();
        }

        private bool ReaderNumberExist(int num) => ReadersNumbers.Contains(num);

        private void LoadStartTime()
        {
            if (Player.StartTime == null || Player.IsStartTimeFromReader)
            {
                Player.StartTime = FirstValidTimeRead()?.Time.ToDateTime();
                Player.IsStartTimeFromReader = true;
            }
            if (Player.StartTime == null)
                return;

            StartTime = Player.StartTime.Value.ToDecimal();
        }

        private TimeRead FirstValidTimeRead()
        {
            return TimeReads.FirstOrDefault(i => i.Gate.Number == GateOrderItem[0].Gate.Number);
        }

        protected virtual IEnumerable<GatesOrderItem> GatesOrderNumbers() => GateOrderItem;

        protected bool IsNonsignificantBefore(TimeRead timeRead) => timeRead.Time < StartTime;

        protected virtual bool IsNonsignificantAfter(TimeRead timeRead) => NoReadersRemaining;

        protected virtual bool IsRankable() => NoReadersRemaining;

        protected bool IsRepeated(TimeRead timeRead)
        {
            if (LastSignificant == null)
                return false;
            return timeRead.Time - LastSignificant.Time <
                   TimeProcess.MinRepeatSeconds;
        }

        protected bool IsSkipped(TimeRead timeRead)
        {
            if (LastSignificant == null)
            {
                return false;
            }
            return timeRead.Time - LastSignificant.Time < ExpectedReader.Current?.MinTimeBefore;
        }

        protected bool IsSignificant(TimeRead timeRead) => timeRead.Gate.Number == ExpectedReader.Current?.Gate.Number;

        private bool LoopBody(TimeRead timeRead)
        {
            if(timeRead.TimeReadTypeEnum == TimeReadTypeEnum.Void) return true;
            if(IsNonsignificantBefore(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.NonsignificantBefore;
            else if(IsNonsignificantAfter(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.NonsignificantAfter;
            else if(IsRepeated(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.Repeated;
            else if(IsSkipped(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.Skipped;
            else if (IsSignificant(timeRead))
            {
                timeRead.TimeReadTypeEnum = TimeReadTypeEnum.Significant;
                LastSignificant = timeRead;
                NoReadersRemaining = !ExpectedReader.MoveNext();
            }
            else
            {
                CreateVoidRead();
                return false;
            }
            return true;
        }

        private void CreateVoidRead()
        {
            TimeRead tr = new TimeRead((LastSignificant?.Time ?? StartTime) + ExpectedReader.Current?.MinTimeBefore ?? 0)
            {
                TimeReadTypeEnum = TimeReadTypeEnum.Void,
                GateId = ExpectedReader.Current?.GateId ?? -1
            };
            ExistingVoids.Add(tr);
            LastSignificant = tr;
            NoReadersRemaining = !ExpectedReader.MoveNext();
        }

        private async Task Initialize()
        {
            await RemoveVoids();
            await LoadTimeReads();
            LoadStartTime();
        }

        private Task RemoveVoids()
        {
            return new TimeReadRepository(new ContextProvider(), Player).RemoveVoidsAsync();
        }
    }
}