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
using ViewCore.Managers;

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


        private async void OnViewLoaded()
        {
            ColorLegendManager colorLegendManager = new ColorLegendManager();
            LegendItems = colorLegendManager.GetLegendItems();
            PlayerWithLogsManager playerWithLogsManager = new PlayerWithLogsManager(_currentCompetition, _playerRepository);
            PlayersWithLogs = await playerWithLogsManager.GetAllPlayers();
            foreach (var player in PlayersWithLogs)
                player.DownloadTimeReads();
        }
        #endregion
    }
}
