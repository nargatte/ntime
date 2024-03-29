﻿using System.Linq;
using ViewCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore.Entities;
using System.Windows;
using BaseCore.DataBase;
using BaseCore.TimesProcess;
using ViewCore.ManagersDesktop;

namespace AdminView.Logs
{
    class LogsViewModel : TabItemViewModel
    {
        public LogsViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Logi";
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            ProcessLogs = new RelayCommand(OnProcessLogs);
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

        public RelayCommand ProcessLogs { get; set; }
        public RelayCommand ReloadLogs { get; set; }

        private bool _onlySignificant;
        public bool OnlySignificant
        {
            get { return _onlySignificant; }
            set
            {
                SetProperty(ref _onlySignificant, value);
                OnReloadlogs();
            }
        }

        #endregion

        #region Events and Commands
        public RelayCommand ViewLoadedCmd { get; set; }


        private void OnViewLoaded()
        {
            ColorLegendManagerDesktop colorLegendManager = new ColorLegendManagerDesktop();
            LegendItems = colorLegendManager.GetLegendItems();
            OnReloadlogs();
        }

        private async void OnReloadlogs()
        {
            PlayerWithLogsManagerDesktop playerWithLogsManager = new PlayerWithLogsManagerDesktop(_currentCompetition, _playerRepository);
            PlayersWithLogs = await playerWithLogsManager.GetAllPlayers(OnlySignificant);

            PlayersWithLogs = new ObservableCollection<EditablePlayerWithLogs>(PlayersWithLogs.OrderBy(p => p.StartNumber)); //TO REMOVE presentation purpose
        }

        private async void OnProcessLogs()
        {
            TimeProcessManager timeProcess = new TimeProcessManager(_currentCompetition.DbEntity);
            await timeProcess.ProcessAllAsync();
            OnReloadlogs();
            MessageBox.Show("Przeliczono logi");
        }
        #endregion
    }
}
