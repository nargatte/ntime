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
using System.Windows;

namespace AdminView.Categories
{
    class CategoriesViewModel : TabItemViewModel
    {
        public CategoriesViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            NewCategory = new EditableAgeCategory(currentCompetition);
            NewSubcategory = new EditableSubcategory(_currentCompetition);
            TabTitle = "Kategorie";
            AddCategoryCmd = new RelayCommand(OnAddCategoryAsync);
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddSubcategoryCmd = new RelayCommand(OnAddSubcategoryAsync);
        }


        private async void OnViewLoadedAsync()
        {
            await DownloadCategoriesFromDatabase();
            await DownloadSubcategoriesFromDatabase();
        }


        #region Categories
        private async Task DownloadCategoriesFromDatabase(bool clearDisplayedCategoriesBefore = true)
        {
            if (clearDisplayedCategoriesBefore)
                Categories.Clear();
            var dbAgeCategories = await _ageCategoryRepository.GetAllAsync();
            foreach (var dbAgeCategory in dbAgeCategories)
            {
                var categoryToAdd = new EditableAgeCategory(_currentCompetition)
                {
                    DbEntity = dbAgeCategory
                };
                categoryToAdd.UpdateRequested += Category_UpdateRequestedAsync;
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
            if (String.IsNullOrWhiteSpace(NewCategory.Name) ||
                String.IsNullOrWhiteSpace(NewCategory.YearFrom.ToString()) || 
                String.IsNullOrWhiteSpace(NewCategory.YearTo.ToString()) ||
                !int.TryParse(NewCategory.YearFrom.ToString(), out int result1) ||
                !int.TryParse(NewCategory.YearFrom.ToString(), out int result2))
            {
                MessageBox.Show("Kategorie i lata graniczne nie mogę być puste");
                return;
            }
            var categoryToAdd = NewCategory;
            categoryToAdd.UpdateRequested += Category_UpdateRequestedAsync;
            AddCategoryToGUI(categoryToAdd);
            NewCategory = new EditableAgeCategory(_currentCompetition);
            await _ageCategoryRepository.AddAsync(categoryToAdd.DbEntity);
        }

        private async void Category_UpdateRequestedAsync(object sender, EventArgs e)
        {
            var categoryToEdit = sender as EditableAgeCategory;
            await _ageCategoryRepository.UpdateAsync(categoryToEdit.DbEntity);
        }

        //Might not work
        private async void Category_DeleteRequestedAsync(object sender, EventArgs e)
        {
            var categoryToDelete = sender as EditableAgeCategory;
            Categories.Remove(categoryToDelete);
            await _ageCategoryRepository.RemoveAsync(categoryToDelete.DbEntity);
        }
        #endregion

        #region Subcategory

        private async Task DownloadSubcategoriesFromDatabase(bool clearDisplayedSubcategoriesBefore = true)
        {
            if (clearDisplayedSubcategoriesBefore)
                Subcategories.Clear();
            var dbSubcategories = await SubcategoryRepository.GetAllAsync();
            foreach (var dbSubcategory in dbSubcategories)
            {
                var subcategoryToAdd = new EditableSubcategory(_currentCompetition)
                {
                    DbEntity = dbSubcategory
                };
                subcategoryToAdd.UpdateRequested += Subcategory_UpdateRequestedAsync;
                AddSubcategoryToGUI(subcategoryToAdd);
            }
        }


        private void AddSubcategoryToGUI(EditableSubcategory subcategoryToAdd)
        {
            subcategoryToAdd.DeleteRequested += Subcategory_DeleteRequestedAsync;
            Subcategories.Add(subcategoryToAdd);
        }

        private async void OnAddSubcategoryAsync()
        {
            if (String.IsNullOrWhiteSpace(NewSubcategory.Name) || String.IsNullOrWhiteSpace(NewSubcategory.ShortName))
            {
                MessageBox.Show("Dodatkowe informacje dla zawodnika muszą posiadać zarówno pełną nazwę jak i skróconą");
                return;
            }
            var subcategoryToAdd = NewSubcategory;
            subcategoryToAdd.UpdateRequested += Subcategory_UpdateRequestedAsync;
            AddSubcategoryToGUI(subcategoryToAdd);
            await SubcategoryRepository.AddAsync(subcategoryToAdd.DbEntity);
            NewSubcategory = new EditableSubcategory(_currentCompetition);
        }

        private async void Subcategory_UpdateRequestedAsync(object sender, EventArgs e)
        {
            var subcategoryToUpdate = sender as EditableSubcategory;
            await SubcategoryRepository.UpdateAsync(subcategoryToUpdate.DbEntity);
        }

        private async void Subcategory_DeleteRequestedAsync(object sender, EventArgs e)
        {
            var subcategoryToDelete = sender as EditableSubcategory;
            Subcategories.Remove(subcategoryToDelete);
            await SubcategoryRepository.RemoveAsync(subcategoryToDelete.DbEntity);
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


        private EditableSubcategory _newSubcategory;
        public EditableSubcategory NewSubcategory
        {
            get { return _newSubcategory; }
            set
            {
                SetProperty(ref _newSubcategory, value);
                NewSubcategory.DbEntity.Name = _newSubcategory.Name;
                NewSubcategory.DbEntity.ShortName = _newSubcategory.ShortName;
            }
        }


        private ObservableCollection<EditableSubcategory> _subcategories = new ObservableCollection<EditableSubcategory>();
        public ObservableCollection<EditableSubcategory> Subcategories
        {
            get { return _subcategories; }
            set { SetProperty(ref _subcategories, value); }
        }
        #endregion

        public event Action CompetitionManagerRequested = delegate { };
        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand AddCategoryCmd { get; private set; }
        public RelayCommand DeleteCategoryCmd { get; private set; }
        public RelayCommand AddSubcategoryCmd { get; set; }
        public RelayCommand DeleteSubcategoryCmd { get; set; }
    }
}
