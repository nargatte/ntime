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
using System.ComponentModel;
using BaseCore.TimesProcess;

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
            PreviousPageCmd = new RelayCommand(OnPreviousPage);
            NextPageCmd = new RelayCommand(OnNextPage);
            TabTitle = "Zawodnicy";
            RecordsRangeInfo.ChildUpdated += RecordsRangeInfo_ChildUpdated;
        }

        #region Properties

        public PlayerFilterOptions PlayerFilter { get; set; } = new PlayerFilterOptions();

        private RangeInfo _recordRangeInfo = new RangeInfo
        { ItemsPerPage = 50, PageNumber = 1, TotalItemsCount = 0 };
        public RangeInfo RecordsRangeInfo
        {
            get { return _recordRangeInfo; }
            set { SetProperty(ref _recordRangeInfo, value); }
        }

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
        public RelayCommand PreviousPageCmd { get; set; }
        public RelayCommand NextPageCmd { get; set; }


        private async void OnPreviousPage()
        {
            if (RecordsRangeInfo.PageNumber > 1)
            {
                RecordsRangeInfo.PageNumber--;
                await AddPlayersFromDatabaseAndDisplay(removeAllDisplayedBefore: true);
            }
        }

        private async void OnNextPage()
        {
            if (RecordsRangeInfo.LastItem < RecordsRangeInfo.TotalItemsCount)
            {
                RecordsRangeInfo.PageNumber++;
                await AddPlayersFromDatabaseAndDisplay(removeAllDisplayedBefore: true);
            }
        }

        private void RecordsRangeInfo_ChildUpdated()
        {
            OnPropertyChanged(nameof(RecordsRangeInfo));
        }


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

        private async void Player_UpdateRequested(object sender, EventArgs e)
        {
            var playerToUpdate = sender as EditablePlayer;
            await _playerRepository.UpdateAsync(playerToUpdate.DbEntity, playerToUpdate.DbEntity.Distance,
                playerToUpdate.DbEntity.ExtraPlayerInfo);
            var updatedPlayer = (await _playerRepository.GetById(playerToUpdate.DbEntity.Id));
            var playerToEdit = Players.First(p => p.DbEntity.Id == playerToUpdate.DbEntity.Id);
            playerToEdit = new EditablePlayer(_currentCompetition)
            {
                DbEntity = updatedPlayer
            };
            OnPropertyChanged(nameof(playerToEdit));
            OnPropertyChanged(nameof(playerToUpdate));
            OnPropertyChanged("Players");

        }

        private async void OnAddPlayerAsync()
        {
            if (CanAddPlayer(NewPlayer, out string message))
            {
                NewPlayer.IsMale = GetSexForPlayer(NewPlayer);
                var playerToAdd = NewPlayer.Clone() as EditablePlayer;
                var tempDistance = playerToAdd.DbEntity.Distance;
                var tempExtraPlayerInfo = playerToAdd.DbEntity.ExtraPlayerInfo;
                await _playerRepository.AddAsync(playerToAdd.DbEntity, playerToAdd.DbEntity.Distance, playerToAdd.DbEntity.ExtraPlayerInfo);
                playerToAdd.DbEntity.Distance = tempDistance;
                playerToAdd.DbEntity.ExtraPlayerInfo = tempExtraPlayerInfo;
                playerToAdd.UpdateRequested += Player_UpdateRequested;
                Players.Add(playerToAdd);
                RecordsRangeInfo.TotalItemsCount++;
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
            newPlayer.StartTime.TryConvertToDateTime(out DateTime startTimeDateTime);
            if (startTimeDateTime < new DateTime(2000, 1, 1))
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
            await AddPlayersFromDatabaseAndDisplay(removeAllDisplayedBefore: true);
        }


        private async void OnUpdateFullCategoriesAsync()
        {
            await _playerRepository.UpdateFullCategoryAllAsync();
            var task = DownloadDataFromDatabaseAsync(true);
            MessageBox.Show("Kategorie zostały przeliczone poprawnie");
            await task;
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
            RecordsRangeInfo.TotalItemsCount = 0;
        }


        private void DeleteSelectedPlayersFromGUI(ICollection<EditablePlayer> selectedPlayersList)
        {
            foreach (var player in selectedPlayersList)
            {
                Players.Remove(player);
            }
            RecordsRangeInfo.TotalItemsCount -= selectedPlayersList.Count;
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

        private async void OnViewLoadedAsync()
        {
            await DownloadDataFromDatabaseAsync(removeAllDisplayedBefore: true);
        }

        private async Task DownloadDataFromDatabaseAsync(bool removeAllDisplayedBefore = false)
        {
            await DownloadDistancesAsync();
            await DownloadExtraPlayerInfoAsync();
            await AddPlayersFromDatabaseAndDisplay(removeAllDisplayedBefore);
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
            RecordsRangeInfo.PageNumber = 1;
            PlayerFilter.Query = FilterGeneral;
            if (SortOrder.HasValue && SortOrder.Value == SortOrderEnum.Descending)
                PlayerFilter.DescendingSort = true;
            if (SortCriteria.HasValue)
                PlayerFilter.PlayerSort = SortCriteria.Value;

            await AddPlayersFromDatabaseAndDisplay(true);
        }
        #endregion

        #region Database

        private async Task AddPlayersFromDatabaseAndDisplay(bool removeAllDisplayedBefore = false)
        {
            var dbPlayers = await DownloadPlayersFromDataBase(PlayerFilter, RecordsRangeInfo.PageNumber - 1, RecordsRangeInfo.ItemsPerPage);
            if (removeAllDisplayedBefore)
                Players = new ObservableCollection<EditablePlayer>();
            DisplayNewPlayers(dbPlayers);
        }

        private async Task<Player[]> DownloadPlayersFromDataBase(PlayerFilterOptions playerFilter, int pageNumber, int numberOfItemsOnPage)
        {
            var dbPlayersTuple = await _playerRepository.GetAllByFilterAsync(playerFilter, pageNumber, numberOfItemsOnPage);
            RecordsRangeInfo.TotalItemsCount = dbPlayersTuple.Item2;
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
                tasks.Add(_playerRepository.RemoveAsync(player.DbEntity));
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
