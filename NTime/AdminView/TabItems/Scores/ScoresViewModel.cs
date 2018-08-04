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
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;
using ViewCore.Factories;

namespace AdminView.Scores
{
    class ScoresViewModel : PlayersViewModelBase
    {
        public ScoresViewModel(IEditableCompetition currentCompetition, DependencyContainer dependencyContainer) : base(currentCompetition, dependencyContainer)
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
            await DownLoadPlayersFromDatabaseAndDisplay();
            MessageBox.Show("Wyniki zostały przeliczone poprawnie");
        }

        private async void OnViewLoadedAsync()
        {
            await DownLoadPlayersFromDatabaseAndDisplay();
        }

        #endregion
    }
}
