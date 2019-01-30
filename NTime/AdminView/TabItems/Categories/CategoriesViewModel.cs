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
using ViewCore.Helpers;
using AdminView.AgeCategoryTemplates;

namespace AdminView.Categories
{
    class CategoriesViewModel : TabItemViewModel
    {
        public CategoriesViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Kategorie";
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);

            NewAgeCategory = new EditableAgeCategory(currentCompetition);
            AddCategoryCmd = new RelayCommand(OnAddCategoryAsync);
            RepeatCategoriesForWomenCmd = new RelayCommand(OnRepeatCategoriesForWomen);
            PairCategoriesWithAllDistancesCmd = new RelayCommand(OnPairCategoriesWithAllDistancesAsync);
            ExportAgeCategoriesToCsvCmd = new RelayCommand(OnExportAgeCategoriesToCsv);
            ImportAgeCategoriesFromCsvCmd = new RelayCommand(OnImportAgeCategoriesFromCsv);
            ClearAgeCategoriesCmd = new RelayCommand(OnClearAgeCategories);
            ImportAgeCategoryTemplateItemsCmd = new RelayCommand(NavToCategoryTemplatesView);

            NewSubcategory = new EditableSubcategory(_currentCompetition);
            AddSubcategoryCmd = new RelayCommand(OnAddSubcategoryAsync);
        }

        private async void OnViewLoadedAsync()
        {
            await DownloadAgeCategoriesFromDatabase(clearDisplayedCategoriesBefore: true);
            await DownloadSubcategoriesFromDatabase(clearDisplayedSubcategoriesBefore: true);
            await DownloadDistancesFromDatabase(clearDisplayedCategoriesBefore: true);
            await DownloadAgeCategoryDistancesFromDatabase(clearDisplayedCategoriesBefore: true);
        }


        #region AgeCategories


        private async void NavToCategoryTemplatesView()
        {
            var categoryTemplatesViewModel = new AgeCategoryTemplatesViewModel();
            var importedAgeCategories = categoryTemplatesViewModel.ShowReadonlyDialog()?.Select(item => new AgeCategory(item));
            if (importedAgeCategories == null)
                MessageBox.Show("Kategorie wiekowe nie zostały zaimportowane z szablonu");
            else
            {
                try
                {
                    await AddAgeCategoriesToDb(importedAgeCategories, _currentCompetition.DbEntity.Id);
                    AddAgeCategoriesRangeToGui(importedAgeCategories);
                    MessageBox.Show("Kategorie zostały wczytane z szablonu. Pamiętaj o połączeniu ich z dystansami");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie udało się dodać kategorii wiekowych do bazy danych: {ex.Message}" +
                        $"{Environment.NewLine}" +
                        $"Inner {ex.InnerException}");
                }
            }
        }

    private async Task DownloadAgeCategoriesFromDatabase(bool clearDisplayedCategoriesBefore = true)
    {
        if (clearDisplayedCategoriesBefore)
            AgeCategories.Clear();
        var dbAgeCategories = await _ageCategoryRepository.GetAllAsync();
        AddAgeCategoriesRangeToGui(dbAgeCategories);
    }

    private void AddAgeCategoriesRangeToGui(IEnumerable<AgeCategory> dbAgeCategories)
    {
        foreach (var dbAgeCategory in dbAgeCategories)
        {
            var categoryToAdd = new EditableAgeCategory(_currentCompetition)
            {
                DbEntity = dbAgeCategory
            };
            AddAgeCategoryToGui(categoryToAdd);
        }
    }

    private void AddAgeCategoryToGui(EditableAgeCategory categoryToAdd)
    {
        categoryToAdd.DeleteRequested += Category_DeleteRequestedAsync;
        categoryToAdd.UpdateRequested += Category_UpdateRequestedAsync;
        AgeCategories.Add(categoryToAdd);
    }

    private async void OnAddCategoryAsync()
    {
        if (String.IsNullOrWhiteSpace(NewAgeCategory.Name) ||
            String.IsNullOrWhiteSpace(NewAgeCategory.YearFrom.ToString()) ||
            String.IsNullOrWhiteSpace(NewAgeCategory.YearTo.ToString()) ||
            !int.TryParse(NewAgeCategory.YearFrom.ToString(), out int result1) ||
            !int.TryParse(NewAgeCategory.YearFrom.ToString(), out int result2))
        {
            MessageBox.Show("Kategorie i lata graniczne nie mogę być puste");
            return;
        }
        var categoryToAdd = NewAgeCategory;
        categoryToAdd.UpdateRequested += Category_UpdateRequestedAsync;
        AddAgeCategoryToGui(categoryToAdd);
        NewAgeCategory = new EditableAgeCategory(_currentCompetition);
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
        AgeCategories.Remove(categoryToDelete);
        await _ageCategoryRepository.RemoveAsync(categoryToDelete.DbEntity);
    }


    private async void OnRepeatCategoriesForWomen()
    {
        if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz stworzyć dla kobiet kopie kategorii męskich?") == MessageBoxResult.Yes)
        {
            var repeatedCategoriesForWomen = AgeCategories
                .Where(ageCategory => ageCategory.Male == true)
                .Where(ageCategory => !AgeCategories.Any(ageCategory2 =>
                    ageCategory2.Male == false &&
                    ageCategory2.YearFrom == ageCategory.YearFrom &&
                    ageCategory2.YearTo == ageCategory.YearTo))
                .Select(ageCategory =>
                    new EditableAgeCategory(ageCategory) { Male = false }
                ).ToList();

            await _ageCategoryRepository.AddRangeAsync(repeatedCategoriesForWomen.Select(category => category.DbEntity));

            await DownloadAgeCategoriesFromDatabase(clearDisplayedCategoriesBefore: true);
        }
    }

    private async void OnPairCategoriesWithAllDistancesAsync()
    {
        if (MessageBoxHelper.DisplayYesNo(
            "Czy na pewno chcesz połączyć kategorie za wszystkimi dystansami? Dotychczas zapisane pary zostaną usunięte!!!") == MessageBoxResult.Yes)
        {
            await _ageCategoryDistanceRepository.RemoveRangeAsync(AgeCategoryDistances);
            AgeCategoryDistances.Clear();

            AddAllAgeCategoryDistancePairs();

            await _ageCategoryDistanceRepository.AddRangeAsync(AgeCategoryDistances);
            await DownloadAgeCategoryDistancesFromDatabase(clearDisplayedCategoriesBefore: true);
        }
    }


    private async void OnExportAgeCategoriesToCsv()
    {
        try
        {
            if (await _ageCategoryRepository.ExportAgeCategoriesToCsv(
                    AgeCategories.Select(editable => editable.DbEntity),
                    Distances.Select(editable => editable.DbEntity)))
            {
                MessageBox.Show("Kategorie wiekowe zostały zapisane poprawnie");
            }
            else
            {
                MessageBox.Show("Nie udało się zapisać kategorii wiekowych");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas zapisywania kategorii wiekowych: {ex.Message}");
        }
    }


    private async void OnImportAgeCategoriesFromCsv()
    {
        try
        {
            var ageCategories = await _ageCategoryRepository.ImportAgeCategoriesFromCsv();
            if (ageCategories == null)
                MessageBox.Show("Nie udało się pobrać kategorii wiekowych");
            try
            {
                await AddAgeCategoriesToDb(ageCategories, _currentCompetition.DbEntity.Id);
                AddAgeCategoriesRangeToGui(ageCategories);
                MessageBox.Show("Kategorie zostały wczytane. Pamiętaj o połączeniu ich z dystansami");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nie udało się dodać kategorii wiekowych do bazy danych: {ex.Message}" +
                    $"{Environment.NewLine}" +
                    $"Inner {ex.InnerException}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Wystąpił błąd podczas wczytywania kategorii wiekowych: {ex.Message}");
        }
    }

    private async Task AddAgeCategoriesToDb(IEnumerable<AgeCategory> ageCategories, int competitionId)
    {
        var readyAgeCategories = new List<AgeCategory>(ageCategories);
        foreach (var ageCategory in readyAgeCategories)
        {
            ageCategory.CompetitionId = competitionId;
        }
        await _ageCategoryRepository.AddRangeAsync(readyAgeCategories);
    }

    private async void OnClearAgeCategories()
    {
        if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz usunąć wszystkie kategorie wiekowe?") == MessageBoxResult.Yes)
        {
            try
            {
                await _ageCategoryRepository.RemoveRangeAsync(AgeCategories.Select(editable => editable.DbEntity));
                AgeCategories.Clear();
                MessageBox.Show("Kategorie wiekowe zostały usunięte");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas usuwania kategorii wiekowych: {ex.Message}" +
                    $"{Environment.NewLine }" +
                    $"Inner: {ex.InnerException}");
            }
        }
    }

    #endregion

    #region Subcategory

    private async Task DownloadSubcategoriesFromDatabase(bool clearDisplayedSubcategoriesBefore = true)
    {
        if (clearDisplayedSubcategoriesBefore)
            Subcategories.Clear();
        var dbSubcategories = await _subcategoryRepository.GetAllAsync();
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
        await _subcategoryRepository.AddAsync(subcategoryToAdd.DbEntity);
        NewSubcategory = new EditableSubcategory(_currentCompetition);
    }

    private async void Subcategory_UpdateRequestedAsync(object sender, EventArgs e)
    {
        var subcategoryToUpdate = sender as EditableSubcategory;
        await _subcategoryRepository.UpdateAsync(subcategoryToUpdate.DbEntity);
    }

    private async void Subcategory_DeleteRequestedAsync(object sender, EventArgs e)
    {
        var subcategoryToDelete = sender as EditableSubcategory;
        Subcategories.Remove(subcategoryToDelete);
        await _subcategoryRepository.RemoveAsync(subcategoryToDelete.DbEntity);
    }
    #endregion


    #region Distances
    private async Task DownloadDistancesFromDatabase(bool clearDisplayedCategoriesBefore = true)
    {
        if (clearDisplayedCategoriesBefore)
            Distances.Clear();
        var dbDistances = await _distanceRepository.GetAllAsync();
        foreach (var dbDistance in dbDistances)
        {
            var distanceToAdd = new EditableDistance(_currentCompetition)
            {
                DbEntity = dbDistance
            };
            Distances.Add(distanceToAdd);
        }
    }

    #endregion

    #region AgeCategoryDistances
    private async Task DownloadAgeCategoryDistancesFromDatabase(bool clearDisplayedCategoriesBefore = true)
    {
        if (clearDisplayedCategoriesBefore)
            AgeCategoryDistances.Clear();
        var dbAgeCategoryDistances = await _ageCategoryDistanceRepository.GetAllAsync();
        foreach (var ageCategoryDistance in dbAgeCategoryDistances)
        {
            AgeCategoryDistances.Add(ageCategoryDistance);
        }
    }

    private void AddAllAgeCategoryDistancePairs()
    {
        foreach (var distance in Distances)
        {
            foreach (var ageCategory in AgeCategories)
            {
                AgeCategoryDistances.Add(new AgeCategoryDistance(_currentCompetition.DbEntity.Id, ageCategory.DbEntity.Id, distance.DbEntity.Id));
            }
        }
        MessageBox.Show("Wszystkie dystanse zostały połączone z kategoriami");
    }

    #endregion


    #region Properties
    private EditableAgeCategory _newAgeCategory;
    public EditableAgeCategory NewAgeCategory
    {
        get { return _newAgeCategory; }
        set
        {
            SetProperty(ref _newAgeCategory, value);
        }
    }

    private ObservableCollection<EditableAgeCategory> _ageCategories = new ObservableCollection<EditableAgeCategory>();
    public ObservableCollection<EditableAgeCategory> AgeCategories
    {
        get { return _ageCategories; }
        set { SetProperty(ref _ageCategories, value); }
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

    private ObservableCollection<EditableDistance> _distances = new ObservableCollection<EditableDistance>();
    public ObservableCollection<EditableDistance> Distances
    {
        get { return _distances; }
        set { SetProperty(ref _distances, value); }
    }

    public List<AgeCategoryDistance> AgeCategoryDistances { get; set; } = new List<AgeCategoryDistance>();


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
    public RelayCommand RepeatCategoriesForWomenCmd { get; private set; }
    public RelayCommand PairCategoriesWithAllDistancesCmd { get; private set; }
    public RelayCommand AddSubcategoryCmd { get; private set; }
    public RelayCommand DeleteSubcategoryCmd { get; private set; }
    public RelayCommand ExportAgeCategoriesToCsvCmd { get; private set; }
    public RelayCommand ImportAgeCategoriesFromCsvCmd { get; private set; }
    public RelayCommand ClearAgeCategoriesCmd { get; private set; }
    public RelayCommand ImportAgeCategoryTemplateItemsCmd { get; private set; }
}
}
