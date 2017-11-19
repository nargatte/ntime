﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using ViewCore.Entities;
using System.Windows;

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel<Player>
    {
        public PlayersViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddPlayerCmd = new RelayCommand(OnAddPlayer);
            DeleteAllPlayersCmd = new RelayCommand(OnDeleteAllPlayersRequestedAsync);
            ReadPlayersFromCsvCmd = new RelayCommand(OnReadPlayersFromCsvAsync);
            TabTitle = "Zawodnicy";
        }

        #region Properties


        private string _filterGeneral;
        public string FilterGeneral
        {
            get { return _filterGeneral; }
            set
            {
                SetProperty(ref _filterGeneral, value);
                FilterValueChangedAsync();
            }
        }

        private EditablePlayer _newPlayer;
        public EditablePlayer NewPlayer
        {
            get { return _newPlayer; }
            set { SetProperty(ref _newPlayer, value); }
        }

        private ObservableCollection<EditablePlayer> _players = new ObservableCollection<EditablePlayer>();
        public ObservableCollection<EditablePlayer> Players
        {
            get { return _players; }
            set { SetProperty(ref _players, value); }
        }



        private ObservableCollection<EditableDistance> _definedDistances = new ObservableCollection<EditableDistance>();
        public ObservableCollection<EditableDistance> DefinedDistances
        {
            get { return _definedDistances; }
            set { SetProperty(ref _definedDistances, value); }
        }


        private ObservableCollection<EditableExtraPlayerInfo> _definedExtraPlayerInfo = new ObservableCollection<EditableExtraPlayerInfo>();
        public ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo
        {
            get { return _definedExtraPlayerInfo; }
            set { SetProperty(ref _definedExtraPlayerInfo, value); }
        }

        #endregion

        #region Methods and Events

        public RelayCommand AddPlayerCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand ReadPlayersFromCsvCmd { get; set; }
        public RelayCommand DeleteAllPlayersCmd { get; set; }

        private void DisplayNewPlayers(Player[] dbPlayers)
        {
            foreach (var dbPlayer in dbPlayers)
            {
                Players.Add(new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
                {
                    DbEntity = dbPlayer,
                    Distance = new EditableDistance(_currentCompetition),
                    ExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition)
                });
            }
        }

        //TODO
        private void OnAddPlayer()
        {
            Players.Add(NewPlayer);
            NewPlayer = new EditablePlayer(_currentCompetition);
        }

        private async void OnReadPlayersFromCsvAsync()
        {
            await AddPlayersFromCsvToDatabase();
            await AddPlayersFromDatabaseAndDisplay(new PlayerFilterOptions(),
                pageNumber: 0, numberOfItemsOnPage: 50);
        }

        private async Task AddPlayersFromCsvToDatabase()
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "CSV files (*.csv)|*.csv";
            if (dialog.ShowDialog().Value)
            {
                var path = dialog.FileName;
                var temp = await _playerRepository.ImportPlayersAsync(path);
                MessageBox.Show($"Odczytano {temp.Item2} zawodników, z czego {temp.Item1} zostało dodanych do bazy");
            }
        }

        private async void OnDeleteAllPlayersRequestedAsync()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć wszystkich {Players.Count} zawodników?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await DeleteAllPlayersFromDatabaseAsync();
                DeleteAllPlayersFromGUI();
                MessageBox.Show("Wszyscy zawodnicy zostali usunięci");
            }
            else return;
        }

        private void DeleteAllPlayersFromGUI()
        {
            Players.Clear();
        }

        private async void OnViewLoadedAsync()
        {
            await DownloadDistancesAsync();
            await DownloadExtraPlayerInfoAsync();
            //await DownloadAllPlayers();
            await AddPlayersFromDatabaseAndDisplay(new PlayerFilterOptions(), 0, 50, true);
            _newPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo);
        }


        private async void FilterValueChangedAsync()
        {
            var filter = new PlayerFilterOptions()
            {
                Query = FilterGeneral
            };
            await AddPlayersFromDatabaseAndDisplay(filter, 0, 50, true);
        }
        #endregion

        #region Database

        private async Task AddPlayersFromDatabaseAndDisplay(PlayerFilterOptions playerFilter,
            int pageNumber, int numberOfItemsOnPage, bool removeAllDisplayedBefore = false)
        {
            var dbPlayers = await DownloadPlayersFromDataBase(playerFilter, pageNumber, numberOfItemsOnPage);
            if (removeAllDisplayedBefore)
                Players = new ObservableCollection<EditablePlayer>();
            DisplayNewPlayers(dbPlayers);
        }

        private async Task<Player[]> DownloadPlayersFromDataBase(PlayerFilterOptions playerFilter, int pageNumber, int numberOfItemsOnPage)
        {
            var dbPlayersTuple = await _playerRepository.GetAllByFilterAsync(playerFilter, pageNumber, numberOfItemsOnPage);
            return dbPlayersTuple.Item1;
        }

        private async Task DeleteAllPlayersFromDatabaseAsync()
        {
            await _playerRepository.RemoveAllAsync();
        }

        private async Task DownloadDistancesAsync()
        {
            var dbDistances = await _distanceRepository.GetAllAsync();
            if (dbDistances.Length > 0)
                foreach (var dbDistance in dbDistances)
                {
                    DefinedDistances.Add(new EditableDistance(_currentCompetition)
                    {
                        DbEntity = dbDistance
                    });
                }
        }

        private async Task DownloadExtraPlayerInfoAsync()
        {
            var dbExtraPlayerInfos = await _extraPlayerInfoRepository.GetAllAsync();
            foreach (var item in dbExtraPlayerInfos)
            {
                DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
                {
                    DbEntity = item
                });
            }
        }
        #endregion
    }
}
