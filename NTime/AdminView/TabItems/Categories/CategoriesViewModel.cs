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
            TabTitle = "Kategorie";
            AddCategoryCmd = new RelayCommand(OnAddCategoryAsync);
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
        }

        private async void OnViewLoadedAsync()
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

        private async void Category_DeleteRequestedAsync(object sender, EventArgs e)
        {
            var categoryToDelete = sender as EditableAgeCategory;
            Categories.Remove(categoryToDelete);
            await _ageCategoryRepository.RemoveAsync(categoryToDelete.DbEntity);
        }

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

        public event Action CompetitionManagerRequested = delegate { };
        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand AddCategoryCmd { get; private set; }
        public RelayCommand DeleteCategoryCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; set; }
    }
}
