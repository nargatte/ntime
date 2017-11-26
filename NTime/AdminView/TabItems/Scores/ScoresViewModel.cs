using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;
using ViewCore.Entities;
using BaseCore.PlayerFilter;
using System.Windows;

namespace AdminView.Scores
{
    class ScoresViewModel : PlayersViewModelBase
    {
        public ScoresViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Wyniki";
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            UpdateRankingAllCmd = new RelayCommand(OnUpdateRankingAllAsync);
        }

        #region Events and commands

        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand UpdateRankingAllCmd { get; set; }

        private async void OnUpdateRankingAllAsync()
        {
            await _playerRepository.UpdateRankingAllAsync();
            await DownLoadDataFromDatabaseAndDisplay();
            MessageBox.Show("Wyniki zostały przeliczone poprawnie");
        }

        private async void OnViewLoadedAsync()
        {
            await DownLoadDataFromDatabaseAndDisplay();
        }

        #endregion
    }
}
