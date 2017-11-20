using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using BaseCore.DataBase;

namespace AdminView.Categories
{
    class CategoriesViewModel : TabItemViewModel<AgeCategory>
    {
        public CategoriesViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            NewCategory = new EditableAgeCategory(currentCompetition);
            NewExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition);
            TabTitle = "Kategorie";
            AddCategoryCmd = new RelayCommand(OnAddCategoryAsync);
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddExtraPlayerInfoCmd = new RelayCommand(OnAddExtraPlayerInfoAsync);
        }


        private async void OnViewLoadedAsync()
        {
            await DownloadCategoriesFromDatabase();
            await DownloadExtraPlayerInfosFromDatabase();
        }


        #region Categories
        private async Task DownloadCategoriesFromDatabase()
        {
            var dbAgeCategories = await _ageCategoryRepository.GetAllAsync();
            foreach (var dbAgeCategory in dbAgeCategories)
            {
                var categoryToAdd = new EditableAgeCategory(_currentCompetition)
                {
                    DbEntity = dbAgeCategory
                };
                AddCategoryToGUI(categoryToAdd);
            }
        }

        private void AddCategoryToGUI(EditableAgeCategory categoryToAdd)
        {
            categoryToAdd.DeleteRequested += Category_DeleteRequestedAsync;
            Categories.Add(categoryToAdd);
        }

        private async void OnAddCategoryAsync()
        {
            var categoryToAdd = NewCategory;
            AddCategoryToGUI(categoryToAdd);
            NewCategory = new EditableAgeCategory(_currentCompetition);
            await _ageCategoryRepository.AddAsync(categoryToAdd.DbEntity);
        }

        //Might not work
        private async void Category_DeleteRequestedAsync(object sender, EventArgs e)
        {
            var categoryToDelete = sender as EditableAgeCategory;
            Categories.Remove(categoryToDelete);
            await _ageCategoryRepository.RemoveAsync(categoryToDelete.DbEntity);
        }
        #endregion

        #region ExtraPlayerInfo

        private async Task DownloadExtraPlayerInfosFromDatabase()
        {
            var dbExtraPlayerInfos = await _extraPlayerInfoRepository.GetAllAsync();
            foreach (var dbExtraPlayerInfo in dbExtraPlayerInfos)
            {
                var extraPlayerInfoToAdd = new EditableExtraPlayerInfo(_currentCompetition)
                {
                    DbEntity = dbExtraPlayerInfo
                };
                AddExtraPlayerInfoToGUI(extraPlayerInfoToAdd);
            }
        }


        private void AddExtraPlayerInfoToGUI(EditableExtraPlayerInfo extraPlayerInfoToAdd)
        {
            extraPlayerInfoToAdd.DeleteRequested += ExtraPlayerInfo_DeleteRequestedAsync;
            ExtraPlayerInfos.Add(extraPlayerInfoToAdd);
        }

        private async void OnAddExtraPlayerInfoAsync()
        {
            var extraPlayerInfoToAdd = NewExtraPlayerInfo;
            AddExtraPlayerInfoToGUI(extraPlayerInfoToAdd);
            NewExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition);
            await _extraPlayerInfoRepository.AddAsync(extraPlayerInfoToAdd.DbEntity);
        }

        private async void ExtraPlayerInfo_DeleteRequestedAsync(object sender, EventArgs e)
        {
            var extraPlayerInfoToDelete = sender as EditableExtraPlayerInfo;
            ExtraPlayerInfos.Remove(extraPlayerInfoToDelete);
            await _extraPlayerInfoRepository.RemoveAsync(extraPlayerInfoToDelete.DbEntity);
        }
        #endregion

        #region Properties
        private EditableAgeCategory _newCategory;
        public EditableAgeCategory NewCategory
        {
            get { return _newCategory; }
            set
            {
                SetProperty(ref _newCategory, value);
                _newCategory.DbEntity.Name = _newCategory.Name;
                _newCategory.DbEntity.YearFrom = _newCategory.YearFrom;
                _newCategory.DbEntity.YearTo = _newCategory.YearTo;
            }
        }

        private ObservableCollection<EditableAgeCategory> _categories = new ObservableCollection<EditableAgeCategory>();
        public ObservableCollection<EditableAgeCategory> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }


        private EditableExtraPlayerInfo _newExtraPlayerInfo;
        public EditableExtraPlayerInfo NewExtraPlayerInfo
        {
            get { return _newExtraPlayerInfo; }
            set
            {
                SetProperty(ref _newExtraPlayerInfo, value);
                NewExtraPlayerInfo.DbEntity.Name = _newExtraPlayerInfo.Name;
                NewExtraPlayerInfo.DbEntity.ShortName = _newExtraPlayerInfo.ShortName;
            }
        }


        private ObservableCollection<EditableExtraPlayerInfo> _extraPlayerInfos = new ObservableCollection<EditableExtraPlayerInfo>();
        public ObservableCollection<EditableExtraPlayerInfo> ExtraPlayerInfos
        {
            get { return _extraPlayerInfos; }
            set { SetProperty(ref _extraPlayerInfos, value); }
        }
        #endregion

        public event Action CompetitionManagerRequested = delegate { };
        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand AddCategoryCmd { get; private set; }
        public RelayCommand DeleteCategoryCmd { get; private set; }
        public RelayCommand AddExtraPlayerInfoCmd { get; set; }
        public RelayCommand DeleteExtraPlayerInfoCmd { get; set; }
    }
}
