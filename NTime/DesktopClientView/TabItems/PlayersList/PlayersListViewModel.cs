﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;

namespace DesktopClientView.TabItems.PlayersList
{
    public class PlayersListViewModel : PlayersViewModelBase, ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;

        public PlayersListViewModel(EditableCompetition currentCompetition, DependencyContainer dependencyContainer) :base(currentCompetition, dependencyContainer)
        {
            OnCreation();
        }

        private void OnCreation()
        {
            TabTitle = "Lista zawodników";
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            //UpdateRankingAllCmd = new RelayCommand(OnUpdateRankingAllAsync);
            _competitionData = new CompetitionChoiceBase(_dependencyContainer);
        }

        #region Events and commands

        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand UpdateRankingAllCmd { get; set; }


        //private async void OnUpdateRankingAllAsync()
        //{
        //    await _playerRepository.UpdateRankingAllAsync();
        //    await DownLoadPlayersFromDatabaseAndDisplay();
        //    MessageBox.Show("Wyniki zostały przeliczone poprawnie");
        //}

        private void OnViewLoaded()
        {
            DetachAllEvents();
            CompetitionData.DownloadCompetitionsFromDatabaseAndDisplay();
            CompetitionData.CompetitionSelected += OnCompetitionSelectedAsync;
        }

        private async void OnCompetitionSelectedAsync()
        {
            if (CompetitionData.IsCompetitionSelected)
            {
                await DownLoadPlayersFromDatabaseAndDisplay(CompetitionData.SelectedCompetition);
            }
        }

        public void DetachAllEvents()
        {
            CompetitionData.DetachAllEvents();
        }
        #endregion

    }
}
