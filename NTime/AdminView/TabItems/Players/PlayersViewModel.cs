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
using ViewCore.Managers;

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel
    {
        PlayersManager _playersManager;
        DistancesManager _distancesManager;
        ExtraPlayerInfosManager _extraPlayerInfosManager;

        public PlayersViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddPlayerCmd = new RelayCommand(OnAddPlayerAsync);
            DeleteAllPlayersCmd = new RelayCommand(OnDeleteAllPlayersRequestedAsync);
            ReadPlayersFromCsvCmd = new RelayCommand(OnReadPlayersFromCsvAsync);
            UpdateFullCategoriesCmd = new RelayCommand(OnUpdateFullCategoriesAsync);
            DeleteSelectedPlayersCmd = new RelayCommand(OnDeleteSelectedPlayersRequestedAsync);
            PreviousPageCmd = new RelayCommand(OnPreviousPageAsync);
            NextPageCmd = new RelayCommand(OnNextPageAsync);
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


        private async void OnViewLoadedAsync()
        {
            _distancesManager = new DistancesManager(_currentCompetition);
            DefinedDistances = await _distancesManager.DownloadDistancesAsync();

            _extraPlayerInfosManager = new ExtraPlayerInfosManager(_currentCompetition);
            DefinedExtraPlayerInfo = await _extraPlayerInfosManager.DownloadExtraPlayerInfoAsync();

            _playersManager = new PlayersManager(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo);
            await _playersManager.AddPlayersFromDatabase(removeAllDisplayedBefore: true);

            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
            ClearNewPlayer();
            //await DownloadDataFromDatabaseAsync(removeAllDisplayedBefore: true);
        }

        private async void OnAddPlayerAsync()
        {
            await _playersManager.TryAddPlayerAsync(NewPlayer);
            ClearNewPlayer();
        }

        private async void OnPreviousPageAsync()
        {
            await _playersManager.NavToPreviousPageAsync();
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        private async void OnNextPageAsync()
        {
            await _playersManager.NavToNextPageAsync();
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        private void UpdateRecordsRangeInfo(RangeInfo rangeInfo)
        {
            RecordsRangeInfo.TotalItemsCount = rangeInfo.TotalItemsCount;
            RecordsRangeInfo.ItemsPerPage = rangeInfo.ItemsPerPage;
            RecordsRangeInfo.PageNumber = rangeInfo.PageNumber;
        }

        private void RecordsRangeInfo_ChildUpdated()
        {
            OnPropertyChanged(nameof(RecordsRangeInfo));
        }


        //private void DisplayNewPlayers(Player[] dbPlayers)
        //{
        //    foreach (var dbPlayer in dbPlayers)
        //    {
        //        var playerToAdd = new EditablePlayer(dbPlayer, _currentCompetition, DefinedDistances, DefinedExtraPlayerInfo);
        //        playerToAdd.UpdateRequested += Player_UpdateRequested;
        //        Players.Add(playerToAdd);
        //    }
        //}



        private async void OnReadPlayersFromCsvAsync()
        {
            await _playersManager.AddPlayersFromCsvToDatabase();
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }


        private async void OnUpdateFullCategoriesAsync()
        {
            var task = _playerRepository.UpdateFullCategoryAllAsync();
            MessageBox.Show("Kategorie zostały przeliczone poprawnie");
            await task;
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }


        private async void OnDeleteAllPlayersRequestedAsync()
        {
            MessageBoxResult result = MessageBox.Show(
                $"Czy na pewno chcesz usunąć wszystkich zawodników?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                await _playersManager.DeleteAllPlayersFromDatabaseAsync();
                Players = _playersManager.GetPlayersToDisplay();
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
                MessageBox.Show("Wszyscy zawodnicy zostali usunięci");
            }
            else return;
        }


        private void DeleteSelectedPlayersFromGUI(ICollection<EditablePlayer> selectedPlayersList)
        {
            foreach (var player in selectedPlayersList)
            {
                Players.Remove(player);
            }
            //RecordsRangeInfo.TotalItemsCount -= selectedPlayersList.Count;
        }

        private void UpdatePlayers(List<EditablePlayer> playersToUpdate)
        {
            foreach (var player in playersToUpdate)
            {

            }
        }

        private void OnDeleteSelectedPlayersRequestedAsync()
        {
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz usunąć {SelectedPlayersList.Count} zawodników?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var selectedPlayersArray = SelectedPlayersList.Cast<EditablePlayer>().ToArray();
                _playersManager.DeleteSelectedPlayersFromDatabaseAsync(selectedPlayersArray);
                DeleteSelectedPlayersFromGUI(selectedPlayersArray);
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
                MessageBox.Show("Wybrani zawodnicy zostali usunięci");
            }
            else return;
        }

        private void ClearNewPlayer()
        {
            NewPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo, new Player()
            {
                //Distance = new EditableDistance(_currentCompetition),
                //ExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition)
                Distance = new Distance(),
                ExtraPlayerInfo = new ExtraPlayerInfo(),
                StartTime = DateTime.Today,
                BirthDate = DateTime.Today
            });
        }

        private async void FilterValueChangedAsync()
        {
            await _playersManager.UpdateFilterInfo(pageNumber: 1, query: FilterGeneral, sortOrder: SortOrder, sortCriteria: SortCriteria);
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }
        #endregion

    }

}
