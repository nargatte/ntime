using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore;
using System.Collections.ObjectModel;
using MvvmHelper;
using BaseCore.DataBase;
using ViewCore.Entities;
using System.Reflection;
using System.ComponentModel;

namespace AdminView.Logs
{
    class LogsViewModel : TabItemViewModel
    {
        public LogsViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Logi";
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
        }


        #region Properties

        private ObservableCollection<TimeReadColorsLegendItem> _legendItems;
        public ObservableCollection<TimeReadColorsLegendItem> LegendItems
        {
            get { return _legendItems; }
            set { SetProperty(ref _legendItems, value); }
        }

        private ObservableCollection<EditablePlayerWithLogs> _playersWithLogs = new ObservableCollection<EditablePlayerWithLogs>();
        public ObservableCollection<EditablePlayerWithLogs> PlayersWithLogs
        {
            get { return _playersWithLogs; }
            set { SetProperty(ref _playersWithLogs, value); }
        }

        #endregion

        #region Events and Commands
        public RelayCommand ViewLoadedCmd { get; set; }


        private void OnViewLoaded()
        {
            FillLegendCollection();
            DownloadTimeReadsWithPlayers();
        }

        private async void DownloadTimeReadsWithPlayers()
        {
            var dbPlayers = await _playerRepository.GetAllAsync();
            foreach (var dbPlayer in dbPlayers)
            {
                EditablePlayerWithLogs playerToAdd = new EditablePlayerWithLogs(_currentCompetition)
                {
                    DbEntity = dbPlayer
                };
                playerToAdd.DownloadTimeReads();
                PlayersWithLogs.Add(playerToAdd);
            }
        }

        private void FillLegendCollection()
        {
            LegendItems = new ObservableCollection<TimeReadColorsLegendItem>();
            var enumValues =  Enum.GetValues(typeof(TimeReadTypeEnum)) as TimeReadTypeEnum[];
            for (int i = 0; i < enumValues.Length; i++)
            {
                LegendItems.Add(new TimeReadColorsLegendItem()
                {
                    Name = GetEnumDescription(enumValues[i]),
                    TimeReadType = enumValues[i]
                });
            }
        }
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
        #endregion
    }
}
