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
            AddPlayerCmd = new RelayCommand(OnAddPlayerAsync);
            DeleteAllPlayersCmd = new RelayCommand(OnDeleteAllPlayersRequestedAsync);
            ReadPlayersFromCsvCmd = new RelayCommand(OnReadPlayersFromCsvAsync);
            TabTitle = "Zawodnicy";
            //NewPlayer = new EditablePlayer(_currentCompetition)
            //{
            //    DbEntity = new Player()
            //    {
            //        Distance = new Distance(),
            //        ExtraPlayerInfo = new ExtraPlayerInfo()
            //    }
            //};
        }

        #region Properties


        private PlayerSort? _sortCriteria;
        public PlayerSort? SortCriteria
        {
            get { return _sortCriteria; }
            set
            {
                SetProperty(ref _sortCriteria, value);
                FilterValueChangedAsync();
            }
        }


        private SortOrderEnum? _sortOrder;
        public SortOrderEnum? SortOrder
        {
            get { return _sortOrder; }
            set
            {
                SetProperty(ref _sortOrder, value);
                FilterValueChangedAsync();
            }
        }


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

        private async void OnAddPlayerAsync()
        {
            if (CanAddPlayer(NewPlayer, out string message))
            {
                //NewPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
                //{
                //    DbEntity = new Player()
                //};
                await _playerRepository.AddAsync(NewPlayer.DbEntity, NewPlayer.DbEntity.Distance, NewPlayer.DbEntity.ExtraPlayerInfo);
                Players.Add(NewPlayer);
            }
            else
            {
                MessageBox.Show(message);
            }

        }

        private bool CanAddPlayer(EditablePlayer newPlayer, out string message)
        {
            message = "";
            if (String.IsNullOrWhiteSpace(newPlayer.LastName))
            {
                message = "Nazwisko nie może być puste";
                return false;
            }
            if (newPlayer.StartTime < new DateTime(2000, 1, 1))
            {
                message = "Nieprawidłowy czas startu zawodnika";
                return false;
            }
            //if(newPlayer.Distance == null)
            //{
            //    message = "Nie przypisano żadnego dystansu";
            //    return false;
            //}

            return true;
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
            NewPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
            {
                //Distance = new EditableDistance(_currentCompetition),
                //ExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition)
                DbEntity = new Player()
                {
                    Distance = new Distance(),
                    ExtraPlayerInfo = new ExtraPlayerInfo(),
                    StartTime = DateTime.Today,
                    BirthDate= DateTime.Today
                }
            };
        }


        private async void FilterValueChangedAsync()
        {
            var filter = new PlayerFilterOptions();
            if (!String.IsNullOrWhiteSpace(FilterGeneral))
                filter.Query = FilterGeneral;
            if (SortOrder.HasValue && SortOrder.Value == SortOrderEnum.Descending)
                filter.DescendingSort = true;
            if (SortCriteria.HasValue)
                filter.PlayerSort = SortCriteria.Value;

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
