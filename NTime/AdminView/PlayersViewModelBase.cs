using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Managers;

namespace AdminView
{
    abstract class PlayersViewModelBase : TabItemViewModel
    {
        protected PlayersManager _playersManager;
        protected DistancesManager _distancesManager;
        protected ExtraPlayerInfosManager _extraPlayerInfosManager;

        protected PlayersViewModelBase(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            UpdateFullCategoriesCmd = new RelayCommand(OnUpdateFullCategoriesAsync);
            PreviousPageCmd = new RelayCommand(OnPreviousPageAsync);
            NextPageCmd = new RelayCommand(OnNextPageAsync);
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


        #region Methods and events

        public RelayCommand UpdateFullCategoriesCmd { get; set; }
        public RelayCommand PreviousPageCmd { get; set; }
        public RelayCommand NextPageCmd { get; set; }

        protected async Task DownLoadDataFromDatabaseAndDisplay()
        {
            _distancesManager = new DistancesManager(_currentCompetition);
            DefinedDistances = await _distancesManager.DownloadDistancesAsync();

            _extraPlayerInfosManager = new ExtraPlayerInfosManager(_currentCompetition);
            DefinedExtraPlayerInfo = await _extraPlayerInfosManager.DownloadExtraPlayerInfoAsync();

            _playersManager = new PlayersManager(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo, RecordsRangeInfo);
            await _playersManager.AddPlayersFromDatabase(removeAllDisplayedBefore: true);

            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        protected async void OnPreviousPageAsync()
        {
            await _playersManager.NavToPreviousPageAsync();
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        protected async void OnNextPageAsync()
        {
            await _playersManager.NavToNextPageAsync();
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        protected void UpdateRecordsRangeInfo(RangeInfo rangeInfo)
        {
            RecordsRangeInfo.TotalItemsCount = rangeInfo.TotalItemsCount;
            RecordsRangeInfo.ItemsPerPage = rangeInfo.ItemsPerPage;
            RecordsRangeInfo.PageNumber = rangeInfo.PageNumber;
        }

        private void RecordsRangeInfo_ChildUpdated()
        {
            OnPropertyChanged(nameof(RecordsRangeInfo));
        }

        protected async void OnUpdateFullCategoriesAsync()
        {
            await _playerRepository.UpdateFullCategoryAllAsync();
            MessageBox.Show("Kategorie zostały przeliczone poprawnie");
            //await task;
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }


        protected void UpdatePlayers(List<EditablePlayer> playersToUpdate)
        {
            foreach (var player in playersToUpdate)
            {

            }
        }

        protected async void FilterValueChangedAsync()
        {
            await _playersManager.UpdateFilterInfo(pageNumber: 1, query: FilterGeneral, sortOrder: SortOrder, sortCriteria: SortCriteria);
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        #endregion
    }
}
