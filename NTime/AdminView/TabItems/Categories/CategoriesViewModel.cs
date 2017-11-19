using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;

namespace AdminView.Categories
{
    class CategoriesViewModel : TabItemViewModel
    {
        public CategoriesViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Kategorie";
            AddCategoryCmd = new RelayCommand(OnAddCategoryAsync);
            _newCategory =  new ViewCore.Entities.EditableAgeCategory(currentCompetition);
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
        }

        private async void OnViewLoadedAsync()
        {
            var dbAgeCategories = await _ageCategoryRepository.GetAllAsync();
            foreach (var dbAgeCategory in dbAgeCategories)
            {
                var categoryToAdd = new ViewCore.Entities.EditableAgeCategory(_currentCompetition)
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
            AddCategoryToGUI(NewCategory);
            NewCategory = new EditableAgeCategory(_currentCompetition);
            await _ageCategoryRepository.AddAsync(NewCategory.DbEntity);
        }

        //Same must be done after import !!!!
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
            set { SetProperty(ref _newCategory, value); }
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
