using System;
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
using System.Collections;

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
            UpdateFullCategoriesCmd = new RelayCommand(OnUpdateFullCategoriesAsync);
            DeleteSelectedPlayersCmd = new RelayCommand(OnDeleteSelectedPlayersRequestedAsync);
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


        private IList _selectedPlayersList = new ArrayList();
        public IList SelectedPlayersList
        {
            get { return _selectedPlayersList; }
            set { SetProperty(ref _selectedPlayersList, value); }
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
        public RelayCommand UpdateFullCategoriesCmd { get; set; }
        public RelayCommand DeleteSelectedPlayersCmd { get; set; }

        private void DisplayNewPlayers(Player[] dbPlayers)
        {
            foreach (var dbPlayer in dbPlayers)
            {
                var playerToAdd = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
                {
                    DbEntity = dbPlayer,
                };
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                Players.Add(playerToAdd);
            }
        }

        private void Player_UpdateRequested(object sender, EventArgs e)
        {
            var playerToUpdate = sender as EditablePlayer;
            _playerRepository.UpdateAsync(playerToUpdate.DbEntity, playerToUpdate.DbEntity.Distance,
                playerToUpdate.DbEntity.ExtraPlayerInfo);
        }

        private async void OnAddPlayerAsync()
        {
            if (CanAddPlayer(NewPlayer, out string message))
            {
                NewPlayer.IsMale =  GetSexForPlayer(NewPlayer);
                var playerToAdd = NewPlayer.Clone() as EditablePlayer;
                var tempDistance = playerToAdd.DbEntity.Distance;
                var tempExtraPlayerInfo = playerToAdd.DbEntity.ExtraPlayerInfo;
                await _playerRepository.AddAsync(playerToAdd.DbEntity, playerToAdd.DbEntity.Distance, playerToAdd.DbEntity.ExtraPlayerInfo);
                playerToAdd.DbEntity.Distance = tempDistance;
                playerToAdd.DbEntity.ExtraPlayerInfo = tempExtraPlayerInfo;
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                Players.Add(playerToAdd);
                ClearNewPlayer();
            }
            else
            {
                MessageBox.Show(message);
            }

        }

        private bool GetSexForPlayer(EditablePlayer newPlayer)
        {
            char[] firstName = newPlayer.FirstName.ToCharArray();
            if (firstName.Last() == 'a' && NewPlayer.FirstName.ToLower() != "kuba")
                return false;
            else
                return true;
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
            if (newPlayer.Distance == null)
            {
                message = "Nie przypisano żadnego dystansu";
                return false;
            }
            if (newPlayer.ExtraPlayerInfo == null)
            {
                message = "Nie przypisano Dodatkowych informacji";
                return false;
            }

            return true;
        }

        private async void OnReadPlayersFromCsvAsync()
        {
            await AddPlayersFromCsvToDatabase();
            await AddPlayersFromDatabaseAndDisplay(new PlayerFilterOptions(),
                pageNumber: 0, numberOfItemsOnPage: 50);
        }


        private async void OnUpdateFullCategoriesAsync()
        {
            await _playerRepository.UpdateFullCategoryAllAsync();
            DownloadDataFromDatabaseAsync();
            MessageBox.Show("Kategorie zostały przeliczone poprawnie");
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
                $"Czy na pewno chcesz usunąć wszystkich zawodników?",
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


        private void DeleteSelectedPlayersFromGUI(ICollection<EditablePlayer> selectedPlayersList)
        {
            foreach (var player in selectedPlayersList)
            {
                Players.Remove(player);
            }
        }

        private async void OnDeleteSelectedPlayersRequestedAsync()
        {
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz usunąć {SelectedPlayersList.Count} zawodników?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedPlayersArray = SelectedPlayersList.Cast<EditablePlayer>().ToArray();
                await DeleteSelectedPlayersFromDatabaseAsync(selectedPlayersArray);
                DeleteSelectedPlayersFromGUI(selectedPlayersArray);
                MessageBox.Show("Wybrani zawodnicy zostali usunięci");
            }
            else return;
        }

        private void OnViewLoadedAsync()
        {
            DownloadDataFromDatabaseAsync(removeAllDisplayedBefore: true);
        }

        private async void DownloadDataFromDatabaseAsync(bool removeAllDisplayedBefore = false)
        {
            await DownloadDistancesAsync();
            await DownloadExtraPlayerInfoAsync();
            //await DownloadAllPlayers();
            await AddPlayersFromDatabaseAndDisplay(new PlayerFilterOptions(), 0, 50, removeAllDisplayedBefore);
            ClearNewPlayer();
        }

        private void ClearNewPlayer()
        {
            NewPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
            {
                //Distance = new EditableDistance(_currentCompetition),
                //ExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition)
                DbEntity = new Player()
                {
                    Distance = new Distance(),
                    ExtraPlayerInfo = new ExtraPlayerInfo(),
                    StartTime = DateTime.Today,
                    BirthDate = DateTime.Today
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


        private async Task DeleteSelectedPlayersFromDatabaseAsync(ICollection<EditablePlayer> selectedPlayersList)
        {
            List<Task> tasks = new List<Task>();
            foreach (var player in selectedPlayersList)
            {
                tasks.Add( _playerRepository.RemoveAsync(player.DbEntity));
            }
            await Task.WhenAll(tasks);
        }

        private async Task DownloadDistancesAsync()
        {
            var dbDistances = await _distanceRepository.GetAllAsync();
            DefinedDistances.Clear();
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
            DefinedExtraPlayerInfo.Clear();
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
