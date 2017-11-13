﻿using System;
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

        protected ReaderOrder[] ReaderOrder;

        protected HashSet<int> ReadersNumbers;

        protected TimeRead[] TimeReads;

        protected HashSet<TimeRead> VoidsToAdd = new HashSet<TimeRead>();

        protected decimal StartTime;

        protected TimeProcess TimeProcess;

        protected int Circuits;

        protected TimeRead LastSignificant;

        protected IEnumerator<ReaderOrder> ExpectedReader;

        protected bool NonReadersRemain;

        protected internal TimeProcessForDistance(Player player, Distance distance, ReaderOrder[] readerOrder, HashSet<int> readersNumbers,  TimeProcess timeProcess)
        {
            Player = player;
            Distance = distance;
            ReaderOrder = readerOrder;
            TimeProcess = timeProcess;
            ReadersNumbers = readersNumbers;
        }

        public async Task FullProcess()
        {
            await Initialize();
            if (Player.StartTime == null)
                return;
            if (!Array.TrueForAll(TimeReads, t => ReaderNumberExist(t.Reader)))
                return;

            ExpectedReader = ReaderOrderNumers().GetEnumerator();

            Array.ForEach(TimeReads, LoopBody);
            Task t1 = UpdateVoids();
            Task t2 = UpdatePlayer();

            await Task.WhenAll(t1, t2);
        }

        private Task UpdateVoids()
        {
            HashSet<TimeRead> voidsInDb = new HashSet<TimeRead>(TimeReads.Where(t => t.TimeReadTypeEnum == TimeReadTypeEnum.Void));
            HashSet<TimeRead> voidsToRemove = new HashSet<TimeRead>(voidsInDb.Where(t => !VoidsToAdd.Contains(t)));
            HashSet<TimeRead> voidsToAdd = new HashSet<TimeRead>(VoidsToAdd.Where(t => !voidsInDb.Contains(t)));

            TimeReadRepository readRepository = new TimeReadRepository(new ContextProvider(), Player);
            Task t1 = readRepository.AddRangeAsync(voidsToAdd);
            Task t2 = readRepository.RemoveRangeAsync(voidsToRemove);

            return Task.WhenAll(t1, t2);
        }

        private Task UpdatePlayer()
        {
            PlayerRepository playerRepository = new PlayerRepository(new ContextProvider(), TimeProcess.Competition);
            Player.LapsCount = Circuits;
            Player.Time = LastSignificant.Time - StartTime;
            return playerRepository.UpdateAsync(Player);
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
            return TimeReads.FirstOrDefault(i => i.Reader == ReaderOrder[0].ReaderNumber);
        }

        protected virtual IEnumerable<ReaderOrder> ReaderOrderNumers() => ReaderOrder;

        protected bool IsNonsignificantBefore(TimeRead timeRead) => timeRead.Time < StartTime;

        protected virtual bool IsNonsignificantAfter(TimeRead timeRead) => NonReadersRemain;

        protected bool IsRepeted(TimeRead timeRead) => timeRead.Time - (LastSignificant?.Time ?? timeRead.Time + TimeProcess.MinRepetSeconds)  <
                                                       TimeProcess.MinRepetSeconds;

        protected bool IsSkiped(TimeRead timeRead)
        {
            if (LastSignificant == null)
            {
                if (Player.IsStartTimeFromReader)
                    return false;
                return timeRead.Time - StartTime < ExpectedReader.Current?.MinTimeBetween;
            }
            return timeRead.Time - LastSignificant.Time < ExpectedReader.Current?.MinTimeBetween;
        }

        protected bool IsSignificant(TimeRead timeRead) => timeRead.Reader == ExpectedReader.Current?.ReaderNumber;

        private void LoopBody(TimeRead timeRead)
        {
            if(timeRead.TimeReadTypeEnum == TimeReadTypeEnum.Void) return;
            if(IsNonsignificantBefore(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.NonsignificantBefore;
            else if(IsNonsignificantAfter(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.NonsignificantAfter;
            else if(IsRepeted(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.Repeated;
            else if(IsSkiped(timeRead)) timeRead.TimeReadTypeEnum = TimeReadTypeEnum.Skipped;
            else if (IsSignificant(timeRead))
            {
                timeRead.TimeReadTypeEnum = TimeReadTypeEnum.Significant;
                LastSignificant = timeRead;
                NonReadersRemain = !ExpectedReader.MoveNext();
            }
            else
            {
                TimeRead tr = new TimeRead(LastSignificant.Time + ExpectedReader.Current?.MinTimeBetween ?? 0,
                    ExpectedReader.Current?.ReaderNumber ?? -1);
                VoidsToAdd.Add(tr);
                LastSignificant = tr;
                NonReadersRemain = !ExpectedReader.MoveNext();
            }
        }

        private async Task Initialize()
        {
            await LoadTimeReads();
            LoadStartTime();
        }
    }
}