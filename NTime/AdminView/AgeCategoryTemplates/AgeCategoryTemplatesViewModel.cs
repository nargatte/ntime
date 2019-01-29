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
using ViewCore.Helpers;

namespace AdminView.AgeCategoryTemplates
{
    public class AgeCategoryTemplatesViewModel : BindableBase
    {
        Window _view;
        private bool _returnAgeCategoryTemplateItems;
        private const string AgeCategoryTemplateName = "Szablon kategorii wiekowych";
        private const string AgeCategoryTemplateItemName = "Kategoria wiekowa należąca do szablonu";
        private AgeCategoryTemplateRepository _ageCategoryTemplateRepository;
        private AgeCategoryTemplateItemRepository _ageCategoryTemplateItemRepository;


        public AgeCategoryTemplatesViewModel()
        {
            _ageCategoryTemplateRepository = new AgeCategoryTemplateRepository(new ContextProvider());
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddAgeCategoryTemplateCmd = new RelayCommand(OnAddAgeCategoryTemplateAsync);
            AddAgeCategoryTemplateItemCmd = new RelayCommand(OnAddAgeCategoryTemplateItemAsync);
            DeleteAgeCategoryTemplateCmd = new RelayCommand(OnDeleteSelectedAgeCategoryTemplate);
            ClearAgeCategoryTemplateItemsCmd = new RelayCommand(OnClearAgeCategoryTemplates);
            RepeatAgeCategoryTemplateItemsForWomenCmd = new RelayCommand(OnRepeatAgeCategoryTemplateItemsForWomen);
            ReturnAgeCategoryTemplateItemsCmd = new RelayCommand(OnReturnAgeCategoryTemplateItems, CanReturnAgeCategoryTemplateItems);
            _newAgeCategoryTemplateItem = new EditableAgeCategoryTemplateItem(IsEditable);
        }

        #region Properties

        private bool _isEditable;
        public bool IsEditable
        {
            get { return _isEditable; }
            set { SetProperty(ref _isEditable, value); }
        }

        private EditableAgeCategoryTemplate _newAgeCategoryTemplate = new EditableAgeCategoryTemplate();
        public EditableAgeCategoryTemplate NewAgeCategoryTemplate
        {
            get { return _newAgeCategoryTemplate; }
            set { SetProperty(ref _newAgeCategoryTemplate, value); }
        }

        private ObservableCollection<EditableAgeCategoryTemplate> _ageCategoryTemplates = new ObservableCollection<EditableAgeCategoryTemplate>();
        public ObservableCollection<EditableAgeCategoryTemplate> AgeCategoryTemplates
        {
            get { return _ageCategoryTemplates; }
            set { SetProperty(ref _ageCategoryTemplates, value); }
        }

        private EditableAgeCategoryTemplate _selectedAgeCategoryTemplate;
        public EditableAgeCategoryTemplate SelectedAgeCategoryTemplate
        {
            get { return _selectedAgeCategoryTemplate; }
            set
            {
                SetProperty(ref _selectedAgeCategoryTemplate, value);
                SelectedAgeCategoryTemplateHasChanged(_selectedAgeCategoryTemplate);
            }
        }


        private ObservableCollection<EditableAgeCategoryTemplateItem> _ageCategoryTemplateItems = new ObservableCollection<EditableAgeCategoryTemplateItem>();
        public ObservableCollection<EditableAgeCategoryTemplateItem> AgeCategoryTemplateItems
        {
            get { return _ageCategoryTemplateItems; }
            set { SetProperty(ref _ageCategoryTemplateItems, value); }
        }

        private EditableAgeCategoryTemplateItem _newAgeCategoryTemplateItem;
        public EditableAgeCategoryTemplateItem NewAgeCategoryTemplateItem
        {
            get { return _newAgeCategoryTemplateItem; }
            set
            {
                SetProperty(ref _newAgeCategoryTemplateItem, value);
                //_newAgeCategory.DbEntity.Name = _newAgeCategory.Name;
                //_newAgeCategory.DbEntity.YearFrom = _newAgeCategory.YearFrom;
                //_newAgeCategory.DbEntity.YearTo = _newAgeCategory.YearTo;
                //_newAgeCategory.DbEntity.Male = _newAgeCategory.Male;
            }
        }

        #endregion

        private async void OnViewLoadedAsync()
        {
            await DownloadAgeCategoryTemplatesFromDatabase();
        }

        public void ShowEditableDialog()
        {
            IsEditable = true;
            ShowDialog();
        }

        public List<AgeCategoryTemplateItem> ShowReadonlyDialog()
        {
            IsEditable = false;
            ShowDialog();
            if (_returnAgeCategoryTemplateItems && SelectedAgeCategoryTemplate != null)
                return AgeCategoryTemplateItems.Select(item => item.DbEntity).ToList();
            else
                return null;
        }

        private void ShowDialog()
        {
            _view = new AgeCategoryTemplatesView() { DataContext = this };
            _view.ShowDialog();
        }

        private void OnReturnAgeCategoryTemplateItems()
        {
            _returnAgeCategoryTemplateItems = true;
            _view.Close();
        }

        private bool CanReturnAgeCategoryTemplateItems() => SelectedAgeCategoryTemplate != null;


        #region AgeCategoryTemplates

        private async void OnAddAgeCategoryTemplateAsync()
        {

            if (String.IsNullOrWhiteSpace(NewAgeCategoryTemplate.Name))
            {
                MessageBox.Show("Nazwa szablonu kategorii nie może być pusta");
                return;
            }
            try
            {
                await _ageCategoryTemplateRepository.AddAsync(NewAgeCategoryTemplate.DbEntity);
                AddAgeCategoryTemplateToGui(NewAgeCategoryTemplate);
                NewAgeCategoryTemplate = new EditableAgeCategoryTemplate();
                MessageBox.Show("Szablon kategorii został dodany");
            }
            catch (Exception ex)
            {
                DisplayInvalidAddMessage(AgeCategoryTemplateName + $". Powód: {ex.Message}");
            }
        }

        private async Task DownloadAgeCategoryTemplatesFromDatabase(bool clearDisplayedTemplatesBefore = true)
        {
            try
            {
                var dbAgeCategoryTemplates = await _ageCategoryTemplateRepository.GetAllAsync();
                if (clearDisplayedTemplatesBefore)
                    AgeCategoryTemplates.Clear();
                AddAgeCategoryTemplatesToGui(dbAgeCategoryTemplates);
            }
            catch (Exception ex)
            {
                DisplayInvalidDownloadMessage(AgeCategoryTemplateName + $". Powód: {ex.Message}");
            }
        }


        private void AddAgeCategoryTemplatesToGui(IEnumerable<AgeCategoryTemplate> ageCategoryTemplates)
        {
            foreach (var dbAgeCategoryTemplate in ageCategoryTemplates)
            {
                var categoryToAdd = new EditableAgeCategoryTemplate()
                {
                    DbEntity = dbAgeCategoryTemplate
                };
                AddAgeCategoryTemplateToGui(categoryToAdd);
            }
        }

        private void AddAgeCategoryTemplateToGui(EditableAgeCategoryTemplate ageCategoryTemplate)
        {
            ageCategoryTemplate.UpdateRequested += AgeCategoryTemplate_UpdateRequested;
            AgeCategoryTemplates.Add(ageCategoryTemplate);
        }

        private async void AgeCategoryTemplate_UpdateRequested(object sender, EventArgs e)
        {
            if (!(sender is EditableAgeCategoryTemplate ageCategoryTemplate))
            {
                DisplayInvalidUpdateMessage(AgeCategoryTemplateName);
                return;
            }

            try
            {
                await _ageCategoryTemplateRepository.UpdateAsync(ageCategoryTemplate.DbEntity);
            }
            catch (Exception ex)
            {
                DisplayInvalidUpdateMessage(AgeCategoryTemplateName + $". Powód: {ex.Message}");
            }
        }

        private async void OnDeleteSelectedAgeCategoryTemplate()
        {
            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz usunąć szablon kategorii?. Zmiana jest nieodwracalna") == MessageBoxResult.Yes)
            {
                if (SelectedAgeCategoryTemplate == null)
                {
                    DisplayInvalidDeleteMessage(AgeCategoryTemplateName);
                    return;
                }

                try
                {
                    await _ageCategoryTemplateRepository.RemoveAsync(SelectedAgeCategoryTemplate.DbEntity);
                    AgeCategoryTemplateItems.Clear();
                    AgeCategoryTemplates.Remove(SelectedAgeCategoryTemplate);
                    SelectedAgeCategoryTemplate = null;
                }
                catch (Exception ex)
                {
                    DisplayInvalidDeleteMessage(AgeCategoryTemplateName + $". Powód: {ex.Message}");
                }
            }
        }

        private async void SelectedAgeCategoryTemplateHasChanged(EditableAgeCategoryTemplate ageCategoryTemplate)
        {
            if (ageCategoryTemplate == null)
            {
                _ageCategoryTemplateItemRepository = null;
            }
            else
            {
                _ageCategoryTemplateItemRepository = new AgeCategoryTemplateItemRepository(new ContextProvider(), ageCategoryTemplate.DbEntity);
                await DownloadAgeCategoryTemplateItemsFromDatabase(clearDisplayedTemplateItemsBefore: true);
            }
        }

        #endregion

        #region AgeCategoryTemplateItems

        private async Task DownloadAgeCategoryTemplateItemsFromDatabase(bool clearDisplayedTemplateItemsBefore = true)
        {
            if (!CheckTemplateItemsRepository())
                return;

            try
            {
                var dbAgeCategoryTemplateItems = await _ageCategoryTemplateItemRepository.GetAllAsync();
                if (clearDisplayedTemplateItemsBefore)
                    AgeCategoryTemplateItems.Clear();
                AddAgeCategoryTemplateItemsToGui(dbAgeCategoryTemplateItems);
            }
            catch (Exception ex)
            {
                DisplayInvalidDownloadMessage(AgeCategoryTemplateItemName + $". Powód: {ex.Message}");
            }
        }




        private void AddAgeCategoryTemplateItemsToGui(IEnumerable<AgeCategoryTemplateItem> ageCategoryTemplateItems)
        {
            foreach (var dbAgeCategoryTemplateItem in ageCategoryTemplateItems)
            {
                var categoryToAdd = new EditableAgeCategoryTemplateItem(IsEditable)
                {
                    DbEntity = dbAgeCategoryTemplateItem
                };
                AddAgeCategoryTemplateItemToGui(categoryToAdd);
            }
        }

        private void AddAgeCategoryTemplateItemToGui(EditableAgeCategoryTemplateItem ageCategoryTemplateItem)
        {
            ageCategoryTemplateItem.UpdateRequested += AgeCategoryTemplateItem_UpdateRequested;
            ageCategoryTemplateItem.DeleteRequested += AgeCategoryTemplateItem_DeleteRequested;
            AgeCategoryTemplateItems.Add(ageCategoryTemplateItem);
        }

        private async void AgeCategoryTemplateItem_UpdateRequested(object sender, EventArgs e)
        {
            if (!CheckTemplateItemsRepository())
                return;

            if (!(sender is EditableAgeCategoryTemplateItem ageCategoryTemplateItemToEdit))
            {
                DisplayInvalidUpdateMessage(AgeCategoryTemplateItemName);
                return;
            }

            try
            {
                await _ageCategoryTemplateItemRepository.UpdateAsync(ageCategoryTemplateItemToEdit.DbEntity);
            }
            catch (Exception ex)
            {
                DisplayInvalidUpdateMessage(AgeCategoryTemplateItemName + $". Powód: {ex.Message}");
            }
        }

        private async void AgeCategoryTemplateItem_DeleteRequested(object sender, EventArgs e)
        {
            if (!CheckTemplateItemsRepository())
                return;

            if (!(sender is EditableAgeCategoryTemplateItem ageCategoryTemplateItemToDelete))
            {
                DisplayInvalidDeleteMessage(AgeCategoryTemplateItemName);
                return;
            }

            try
            {
                await _ageCategoryTemplateItemRepository.RemoveAsync(ageCategoryTemplateItemToDelete.DbEntity);
                AgeCategoryTemplateItems.Remove(ageCategoryTemplateItemToDelete);
            }
            catch (Exception ex)
            {
                DisplayInvalidDeleteMessage(AgeCategoryTemplateItemName + $". Powód: {ex.Message}");
            }
        }

        private async void OnAddAgeCategoryTemplateItemAsync()
        {
            if (!CheckTemplateItemsRepository())
                return;

            if (String.IsNullOrWhiteSpace(NewAgeCategoryTemplateItem.Name))
            {
                MessageBox.Show("Kategoria nie może być pusta");
                return;
            }

            if (String.IsNullOrWhiteSpace(NewAgeCategoryTemplateItem.YearFrom?.ToString()) ||
                String.IsNullOrWhiteSpace(NewAgeCategoryTemplateItem.YearTo?.ToString()) ||
                !int.TryParse(NewAgeCategoryTemplateItem.YearFrom.ToString(), out int tempYearFrom) ||
                !int.TryParse(NewAgeCategoryTemplateItem.YearTo.ToString(), out int tempYearTo))
            {
                MessageBox.Show("Lata graniczne mają nieprawidłowe wartości");
                return;
            }

            if (tempYearFrom > tempYearTo)
            {
                MessageBox.Show("Pierwszy rocznik graniczny nie może być większy niż drugi");
                return;
            }

            // Here we need to add it to the selected template
            try
            {
                await _ageCategoryTemplateItemRepository.AddAsync(NewAgeCategoryTemplateItem.DbEntity);
                AddAgeCategoryTemplateItemToGui(NewAgeCategoryTemplateItem);
                NewAgeCategoryTemplateItem = new EditableAgeCategoryTemplateItem(IsEditable);
            }
            catch (Exception ex)
            {
                DisplayInvalidAddMessage(AgeCategoryTemplateItemName + $". Powód: {ex.Message}");
            }
        }



        private async void OnRepeatAgeCategoryTemplateItemsForWomen()
        {
            if (!CheckTemplateItemsRepository())
                return;

            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz stworzyć dla kobiet kopie kategorii męskich?") == MessageBoxResult.Yes)
            {
                var repeatedAgeCategoryTemplateItemsForWomen = AgeCategoryTemplateItems
                    .Where(ageCategory => ageCategory.Male == true)
                    .Where(ageCategory => !AgeCategoryTemplateItems.Any(ageCategory2 =>
                        ageCategory2.Male == false &&
                        ageCategory2.YearFrom == ageCategory.YearFrom &&
                        ageCategory2.YearTo == ageCategory.YearTo))
                    .Select(ageCategory =>
                        new EditableAgeCategoryTemplateItem(ageCategory) { Male = false }
                    ).ToList();

                await _ageCategoryTemplateItemRepository.AddRangeAsync(repeatedAgeCategoryTemplateItemsForWomen.Select(category => category.DbEntity));

                await DownloadAgeCategoryTemplateItemsFromDatabase(clearDisplayedTemplateItemsBefore: true);
            }
        }

        private bool CheckTemplateItemsRepository()
        {
            if (_ageCategoryTemplateItemRepository == null || SelectedAgeCategoryTemplate == null
                || _ageCategoryTemplateItemRepository.AgeCategoryTemplate.Id != SelectedAgeCategoryTemplate.Id)
            {
                MessageBox.Show("Szablon kategorii wiekowych nie został poprawnie wybrany");
                return false;
            }
            return true;
        }

        private async void OnClearAgeCategoryTemplates()
        {
            if (!CheckTemplateItemsRepository())
                return;

            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz usunąć wszystkie kategorie wiekowe dla tego szablonu?") == MessageBoxResult.Yes)
            {
                try
                {
                    await _ageCategoryTemplateItemRepository.RemoveRangeAsync(AgeCategoryTemplateItems.Select(editable => editable.DbEntity));
                    AgeCategoryTemplateItems.Clear();
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

        private void DisplayInvalidUpdateMessage(string objectName) => MessageBox.Show($"Nie udało się zapisać obiektu: {objectName}");

        private void DisplayInvalidAddMessage(string objectName) => MessageBox.Show($"Nie udało się dodać obiektu: {objectName}");

        private void DisplayInvalidDeleteMessage(string objectName) => MessageBox.Show($"Nie udało się usunąć obiektu: {objectName}");

        private void DisplayInvalidDownloadMessage(string objectName) => MessageBox.Show($"Nie udało się pobrać z bazy obiektu: {objectName}");


        #region Properties and events





        public event EventHandler DeleteRequested = delegate { };

        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand AddAgeCategoryTemplateCmd { get; private set; }
        public RelayCommand AddAgeCategoryTemplateItemCmd { get; private set; }
        public RelayCommand RepeatAgeCategoryTemplateItemsForWomenCmd { get; private set; }
        public RelayCommand ClearAgeCategoryTemplateItemsCmd { get; private set; }
        public RelayCommand DeleteAgeCategoryTemplateCmd { get; private set; }
        public RelayCommand ReturnAgeCategoryTemplateItemsCmd { get; private set; }
        //public RelayCommand DeleteAgeCategoryTemplateItemCmd { get; private set; }

        #endregion
    }
}
