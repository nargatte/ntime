using BaseCore.DataBase;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ViewCore.Entities;

namespace AdminView.AgeCategoryTemplates
{
    public class AgeCategoryTemplatesViewModel : BindableBase
    {
        private const string AgeCategoryCollectionNotSelectedMessage = "Kolecja kategorii wiekowych nie została wybrana";
        private AgeCategoryCollectionRepository _ageCategoryCollectionRepository;
        private AgeCategoryTemplateRepository _ageCategoryTemplateRepository;


        public AgeCategoryTemplatesViewModel()
        {
            _ageCategoryCollectionRepository = new AgeCategoryCollectionRepository(new ContextProvider());
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddCategoryCmd = new RelayCommand(OnAddCategoryAsync);
            RepeatCategoriesForWomenCmd = new RelayCommand(OnRepeatCategoriesForWomen);
            ClearAgeCategoriesCmd = new RelayCommand(OnClearAgeCategories);
        }

        private async void OnViewLoadedAsync()
        {
            await DownloadAgeCategoryCollectionsFromDatabase();
        }


        #region AgeCategories
        private async Task DownloadAgeCategoryCollectionsFromDatabase(bool clearDisplayerCategoriesBefore = true)
        {
            if (clearDisplayerCategoriesBefore)
                AgeCategoryCollections.Clear();
            var dbAgeCategoryCollections = await _ageCategoryCollectionRepository.GetAllAsync();
            AddAgeCategoryCollectionsToGui(dbAgeCategoryCollections);
        }

        private void AddAgeCategoryCollectionsToGui(IEnumerable<AgeCategoryCollection> ageCategoryCollections)
        {
            foreach (var dbAgeCategoryCollection in ageCategoryCollections)
            {
                var categoryToAdd = new EditableAgeCategoryCollection()
                {
                    DbEntity = dbAgeCategoryCollection
                };
                AddAgeCategoryCollectionToGui(categoryToAdd);
            }
        }

        private void AddAgeCategoryCollectionToGui(EditableAgeCategoryCollection ageCategoryCollection)
        {
            // Maybe some events registration as well
            AgeCategoryCollections.Add(ageCategoryCollection);
        }

        private async Task DownloadAgeCategoryTemplatesFromDatabase(bool clearDisplayerCategoriesBefore = true)
        {
            if (_ageCategoryTemplateRepository == null)
                MessageBox.Show(AgeCategoryCollectionNotSelectedMessage);
            if (clearDisplayerCategoriesBefore)
                AgeCategoryCollections.Clear();
            var dbAgeCategoryTemplates = await _ageCategoryTemplateRepository.GetAllAsync();
            AddAgeCategoryTemplatesToGui(dbAgeCategoryTemplates);
        }

        private void AddAgeCategoryTemplatesToGui(IEnumerable<AgeCategoryTemplate> ageCategoryTemplate)
        {
            foreach (var dbAgeCategoryTemplate in ageCategoryTemplate)
            {
                var categoryToAdd = new EditableAgeCategoryTemplate()
                {
                    DbEntity = dbAgeCategoryTemplate
                };
                AddAgeCategoryTemplateToGui(categoryToAdd);
            }
        }

        private void AddAgeCategoryTemplateToGui(EditableAgeCategoryTemplate ageCategoryCollection)
        {
            // Maybe some events registration as well
            AgeCategoryTemplates.Add(ageCategoryCollection);
        }

        //private void AddAgeCategoryToGui(EditableAgeCategory categoryToAdd)
        //{
        //    categoryToAdd.DeleteRequested += Category_DeleteRequestedAsync;
        //    categoryToAdd.UpdateRequested += Category_UpdateRequestedAsync;
        //    AgeCategories.Add(categoryToAdd);
        //}

        private async void OnAddCategoryTemplateAsync()
        {
            if (_ageCategoryTemplateRepository == null)
                MessageBox.Show(AgeCategoryCollectionNotSelectedMessage);

            if (String.IsNullOrWhiteSpace(NewAgeCategoryTemplate.Name) ||
                String.IsNullOrWhiteSpace(NewAgeCategoryTemplate.YearFrom.ToString()) ||
                String.IsNullOrWhiteSpace(NewAgeCategoryTemplate.YearTo.ToString()) ||
                !int.TryParse(NewAgeCategoryTemplate.YearFrom.ToString(), out int result1) ||
                !int.TryParse(NewAgeCategoryTemplate.YearFrom.ToString(), out int result2))
            {
                MessageBox.Show("Kategorie i lata graniczne nie mogę być puste");
                return;
            }
            var categoryToAdd = NewAgeCategoryTemplate;
            //categoryToAdd.UpdateRequested += Category_UpdateRequestedAsync;
            AddAgeCategoryToGui(categoryToAdd);
            NewAgeCategoryTemplate = new EditableAgeCategoryTemplate();
            await _ageCategoryTemplateRepository.AddAsync(categoryToAdd.DbEntity);
        }

        //private async void Category_UpdateRequestedAsync(object sender, EventArgs e)
        //{
        //    var categoryToEdit = sender as EditableAgeCategory;
        //    await _ageCategoryRepository.UpdateAsync(categoryToEdit.DbEntity);
        //}

        //Might not work
        //private async void Category_DeleteRequestedAsync(object sender, EventArgs e)
        //{
        //    var categoryToDelete = sender as EditableAgeCategory;
        //    AgeCategories.Remove(categoryToDelete);
        //    await _ageCategoryRepository.RemoveAsync(categoryToDelete.DbEntity);
        //}


        private async void OnRepeatCategoriesForWomen()
        {
            if (_ageCategoryTemplateRepository == null)
                MessageBox.Show(AgeCategoryCollectionNotSelectedMessage);
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz stworzyć dla kobiet kopie kategorii męskich?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var repeatedCategoriesForWomen = AgeCategoryTemplates
                    .Where(ageCategory => ageCategory.Male == true)
                    .Where(ageCategory => !AgeCategoryTemplates.Any(ageCategory2 =>
                        ageCategory2.Male == false &&
                        ageCategory2.YearFrom == ageCategory.YearFrom &&
                        ageCategory2.YearTo == ageCategory.YearTo))
                    .Select(ageCategory =>
                        new EditableAgeCategoryTemplate(ageCategory) { Male = false }
                    ).ToList();

                await _ageCategoryTemplateRepository.AddRangeAsync(repeatedCategoriesForWomen.Select(category => category.DbEntity));

                await DownloadAgeCategoryTemplatesFromDatabase();
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
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz usunąć wszystkie kategorie wiekowe?",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    await _ageCategoryRepository.RemoveRangeAsync(AgeCategories.Select(editable => editable.DbEntity));
                    AgeCategoryTemplates.Clear();
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


        #region Properties and events


        private EditableAgeCategoryCollection _newAgeCategoryCollection = new EditableAgeCategoryCollection();
        public EditableAgeCategoryCollection NewAgeCategoryCollection
        {
            get { return _newAgeCategoryCollection; }
            set { SetProperty(ref _newAgeCategoryCollection, value); }
        }

        private ObservableCollection<EditableAgeCategoryCollection> _ageCategoryCollections = new ObservableCollection<EditableAgeCategoryCollection>();
        public ObservableCollection<EditableAgeCategoryCollection> AgeCategoryCollections
        {
            get { return _ageCategoryCollections; }
            set { SetProperty(ref _ageCategoryCollections, value); }
        }

        private EditableAgeCategoryCollection _selectedAgeCategoryCollection = new EditableAgeCategoryCollection();
        public EditableAgeCategoryCollection SelectedAgeCategoryCollection
        {
            get { return _selectedAgeCategoryCollection; }
            set { SetProperty(ref _selectedAgeCategoryCollection, value); }
        }


        private ObservableCollection<EditableAgeCategoryTemplate> _ageCategoryTemplates = new ObservableCollection<EditableAgeCategoryTemplate>();
        public ObservableCollection<EditableAgeCategoryTemplate> AgeCategoryTemplates
        {
            get { return _ageCategoryTemplates; }
            set { SetProperty(ref _ageCategoryTemplates, value); }
        }

        private EditableAgeCategoryTemplate _newAgeCategoryTemplate = new EditableAgeCategoryTemplate();
        public EditableAgeCategoryTemplate NewAgeCategoryTemplate
        {
            get { return _newAgeCategoryTemplate; }
            set
            {
                SetProperty(ref _newAgeCategoryTemplate, value);
                //_newAgeCategory.DbEntity.Name = _newAgeCategory.Name;
                //_newAgeCategory.DbEntity.YearFrom = _newAgeCategory.YearFrom;
                //_newAgeCategory.DbEntity.YearTo = _newAgeCategory.YearTo;
                //_newAgeCategory.DbEntity.Male = _newAgeCategory.Male;
            }
        }



        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand AddCategoryCmd { get; private set; }
        public RelayCommand DeleteCategoryCmd { get; private set; }
        public RelayCommand RepeatCategoriesForWomenCmd { get; private set; }
        public RelayCommand ClearAgeCategoriesCmd { get; private set; }

        #endregion
    }
}
