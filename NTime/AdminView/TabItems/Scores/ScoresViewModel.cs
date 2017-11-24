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
    class ScoresViewModel : TabItemViewModel, ISwitchableViewModel
    {
        public ScoresViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Wyniki";
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            UpdateFullCategoriesCmd = new RelayCommand(OnUpdateFullCategoriesAsync);
            UpdateRankingAllCmd = new RelayCommand(OnUpdateRankingAllAsync);
            PreviousPageCmd = new RelayCommand(OnPreviousPage);
            NextPageCmd = new RelayCommand(OnNextPage);
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

        private ObservableCollection<EditablePlayer> _scores = new ObservableCollection<EditablePlayer>();
        public ObservableCollection<EditablePlayer> PlayersWithScore
        {
            get { return _scores; }
            set { SetProperty(ref _scores, value); }
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

        #region Events and commands

        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand UpdateFullCategoriesCmd { get; set; }
        public RelayCommand UpdateRankingAllCmd { get; set; }
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


        private async void FilterValueChangedAsync()
        {
            RecordsRangeInfo.PageNumber = 1;
            PlayerFilter.Query = FilterGeneral;
            if (SortOrder.HasValue && SortOrder.Value == SortOrderEnum.Descending)
                PlayerFilter.DescendingSort = true;
            if (SortCriteria.HasValue)
                PlayerFilter.PlayerSort = SortCriteria.Value;

            await AddPlayersFromDatabaseAndDisplay(removeAllDisplayedBefore: true);
        }

        private async void OnUpdateFullCategoriesAsync()
        {
            await _playerRepository.UpdateFullCategoryAllAsync();
            DownloadDataFromDatabaseAsync(removeAllDisplayedBefore: true);
            MessageBox.Show("Kategorie zostały przeliczone poprawnie");
        }

        private async void OnUpdateRankingAllAsync()
        {
            await _playerRepository.UpdateRankingAllAsync();
            DownloadDataFromDatabaseAsync(removeAllDisplayedBefore: true);
            MessageBox.Show("Wyniki zostały przeliczone poprawnie");
        }

        private void OnViewLoadedAsync()
        {
            DownloadDataFromDatabaseAsync(removeAllDisplayedBefore: true);
        }
        private async void DownloadDataFromDatabaseAsync(bool removeAllDisplayedBefore = false)
        {
            await DownloadDistancesAsync();
            await DownloadExtraPlayerInfoAsync();
            await AddPlayersFromDatabaseAndDisplay(removeAllDisplayedBefore: removeAllDisplayedBefore);
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }


        private void DisplayNewPlayers(Player[] dbPlayers)
        {
            foreach (var dbPlayer in dbPlayers)
            {
                var playerToAdd = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
                {
                    DbEntity = dbPlayer,
                };
                PlayersWithScore.Add(playerToAdd);
            }
        }

        #endregion

        #region Database

        private async Task AddPlayersFromDatabaseAndDisplay(bool removeAllDisplayedBefore = false)
        {
            var dbPlayers = await DownloadPlayersFromDataBase(PlayerFilter, RecordsRangeInfo.PageNumber - 1, RecordsRangeInfo.ItemsPerPage);
            if (removeAllDisplayedBefore)
                PlayersWithScore = new ObservableCollection<EditablePlayer>();
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
