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
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;
using ViewCore.ManagersDesktop;
using ViewCore.ManagersInterfaces;

namespace ViewCore
{
    public abstract class PlayersViewModelBase : TabItemViewModel
    {
        protected IPlayerManager _playersManager;
        protected IDistanceManager _distancesManager;
        protected IExtraPlayerInfoManager _extraPlayerInfosManager;
        protected IAgeCategoryManager _ageCategoryManager;

        protected DependencyContainer _dependencyContainer;

        protected PlayersViewModelBase(IEditableCompetition currentCompetition, DependencyContainer dependencyContainer)
        {
            OnCreation();
            _dependencyContainer = dependencyContainer;
            _distanceSortCriteria = new EditableDistance(currentCompetition);
            _extraPlayerInfoSortCriteria = new EditableExtraPlayerInfo(currentCompetition);
            _ageCategorySortCriteria = new EditableAgeCategory(currentCompetition);
            _currentCompetition = currentCompetition;
        }

        private void OnCreation()
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


        private EditableDistance _distanceSortCriteria;
        public EditableDistance DistanceSortCriteria
        {
            get { return _distanceSortCriteria; }
            set
            {
                SetProperty(ref _distanceSortCriteria, value);
                FilterValueChangedAsync();
            }
        }

        private EditableExtraPlayerInfo _extraPlayerInfoSortCriteria;
        public EditableExtraPlayerInfo ExtraPlayerInfoSortCriteria
        {
            get { return _extraPlayerInfoSortCriteria; }
            set
            {
                SetProperty(ref _extraPlayerInfoSortCriteria, value);
                FilterValueChangedAsync();
            }
        }

        private EditableAgeCategory _ageCategorySortCriteria;
        public EditableAgeCategory AgeCategorySortCriteria
        {
            get { return _ageCategorySortCriteria; }
            set
            {
                SetProperty(ref _ageCategorySortCriteria, value);
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

        private ObservableCollection<EditableAgeCategory> _definedAgeCategories = new ObservableCollection<EditableAgeCategory>();
        public ObservableCollection<EditableAgeCategory> DefinedAgeCategories
        {
            get { return _definedAgeCategories; }
            set { SetProperty(ref _definedAgeCategories, value); }
        }
        #endregion


        #region Methods and events

        public RelayCommand UpdateFullCategoriesCmd { get; set; }
        public RelayCommand PreviousPageCmd { get; set; }
        public RelayCommand NextPageCmd { get; set; }

        protected async Task DownLoadPlayersFromDatabaseAndDisplay(IEditableCompetition selectedCompeititon = null)
        {
            await DownloadPlayersInfo(selectedCompeititon);

            _playersManager = _dependencyContainer.PlayerManagerFactory.CreateInstance(_currentCompetition, DefinedDistances,
                DefinedExtraPlayerInfo, RecordsRangeInfo, _dependencyContainer.User, _dependencyContainer.ConnectionInfo);
            //DistanceSortCriteria = new EditableDistance(_currentCompetition);
            //ExtraPlayerInfoSortCriteria = new EditableExtraPlayerInfo(_currentCompetition);
            await _playersManager.AddPlayersFromDatabase(removeAllDisplayedBefore: true);

            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        public async Task DownloadPlayersInfo(IEditableCompetition selectedCompeititon = null)
        {
            if (selectedCompeititon != null)
            {
                _currentCompetition = selectedCompeititon;
            }

            _distancesManager = _dependencyContainer.DistanceManagerFactory.CreateInstance(_currentCompetition, _dependencyContainer.User, _dependencyContainer.ConnectionInfo);
            DefinedDistances = await _distancesManager.DownloadDistancesAsync();

            _extraPlayerInfosManager = _dependencyContainer.ExtraPlayerInfoManagerFactory.CreateInstance(_currentCompetition, _dependencyContainer.User, _dependencyContainer.ConnectionInfo);
            DefinedExtraPlayerInfo = await _extraPlayerInfosManager.DownloadExtraPlayerInfoAsync();

            _ageCategoryManager = _dependencyContainer.AgeCategoryManagerFactory.CreateInstance(_currentCompetition, _dependencyContainer.User, _dependencyContainer.ConnectionInfo);
            DefinedAgeCategories = await _ageCategoryManager.DownloadAgeCategoriesAsync();
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
            await _playersManager.UpdateFilterInfo(pageNumber: 1, query: FilterGeneral, sortOrder: SortOrder, sortCriteria: SortCriteria,
                                                    distanceSortCriteria: DistanceSortCriteria, extraPlayerInfoSortCriteria: ExtraPlayerInfoSortCriteria,
                                                    ageCategorySortCriteria: AgeCategorySortCriteria);
            Players = _playersManager.GetPlayersToDisplay();
            UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
        }

        #endregion
    }
}
