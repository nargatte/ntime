﻿using BaseCore.DataBase;
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
        private const string AgeCategoryTemplateName = "Szablon kategorii wiekowych";
        private const string AgeCategoryTemplateItemName = "Kategoria wiekowa należąca do szablonu";
        private AgeCategoryTemplateRepository _ageCategoryTemplateRepository;
        private AgeCategoryTemplateItemRepository _ageCategoryTemplateItemsRepository;


        public AgeCategoryTemplatesViewModel()
        {
            _ageCategoryTemplateRepository = new AgeCategoryTemplateRepository(new ContextProvider());
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddAgeCategoryCollectionCmd = new RelayCommand(OnAddAgeCategoryTemplateAsync);
            AddAgeCategoryTemplateCmd = new RelayCommand(OnAddAgeCategoryTemplateItemAsync);
            ClearAgeCategoryTemplatesCmd = new RelayCommand(OnClearAgeCategoryTemplates);
            RepeatAgeCategoryTemplateItemsForWomenCmd = new RelayCommand(OnRepeatAgeCategoryTemplateItemsForWomen);
        }

        private async void OnViewLoadedAsync()
        {
            await DownloadAgeCategoryTemplatesFromDatabase();
        }


        #region AgeCategoryTemplates

        private async void OnAddAgeCategoryTemplateAsync()
        {

            if (String.IsNullOrWhiteSpace(NewAgeCategoryTemplate.Name))
            {
                MessageBox.Show("Nazwa szablonu kategorii nie może być pusta");
                return;
            }
            AddAgeCategoryTemplateToGui(NewAgeCategoryTemplate);
            await _ageCategoryTemplateRepository.AddAsync(NewAgeCategoryTemplate.DbEntity);
            NewAgeCategoryTemplate = new EditableAgeCategoryTemplate();
        }

        private async Task DownloadAgeCategoryTemplatesFromDatabase(bool clearDisplayedTemplatesBefore = true)
        {
            if (clearDisplayedTemplatesBefore)
                AgeCategoryTemplates.Clear();
            var dbAgeCategoryTemplates = await _ageCategoryTemplateRepository.GetAllAsync();
            AddAgeCategoryTemplatesToGui(dbAgeCategoryTemplates);
        }


        private void AddAgeCategoryTemplatesToGui(IEnumerable<AgeCategoryCollection> ageCategoryTemplates)
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
            // Maybe some events registration as well
            ageCategoryTemplate.UpdateRequested += AgeCategoryTemplate_UpdateRequested;
            ageCategoryTemplate.DeleteRequested += AgeCategoryTemplate_DeleteRequested;
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
            catch (Exception)
            {
                DisplayInvalidUpdateMessage(AgeCategoryTemplateName);
            }
        }

        private async void AgeCategoryTemplate_DeleteRequested(object sender, EventArgs e)
        {
            if (!(sender is EditableAgeCategoryTemplate ageCategoryTemplate))
            {
                DisplayInvalidDeleteMessage(AgeCategoryTemplateName);
                return;
            }

            try
            {
                await _ageCategoryTemplateRepository.RemoveAsync(ageCategoryTemplate.DbEntity);
            }
            catch (Exception)
            {
                DisplayInvalidDeleteMessage(AgeCategoryTemplateName);
            }
        }

        #endregion

        #region AgeCategoryTemplateItems

        private async Task DownloadAgeCategoryTemplateItemsFromDatabase(bool clearDisplayedTemplateItemsBefore = true)
        {
            if (CheckForNullTemplateItemsRepository())
                return;

            if (clearDisplayedTemplateItemsBefore)
                AgeCategoryTemplateItems.Clear();
            var dbAgeCategoryTemplateItems = await _ageCategoryTemplateItemsRepository.GetAllAsync();
            AddAgeCategoryTemplateItemsToGui(dbAgeCategoryTemplateItems);
        }




        private void AddAgeCategoryTemplateItemsToGui(IEnumerable<AgeCategoryTemplate> ageCategoryTemplateItems)
        {
            foreach (var dbAgeCategoryTemplateItem in ageCategoryTemplateItems)
            {
                var categoryToAdd = new EditableAgeCategoryTemplateItem()
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
            if (CheckForNullTemplateItemsRepository())
                return;

            if (!(sender is EditableAgeCategoryTemplateItem ageCategoryTemplateItemToEdit))
            {
                DisplayInvalidUpdateMessage(AgeCategoryTemplateItemName);
                return;
            }

            try
            {
                await _ageCategoryTemplateItemsRepository.UpdateAsync(ageCategoryTemplateItemToEdit.DbEntity);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się zapisać kategorii");
            }
        }

        private async void AgeCategoryTemplateItem_DeleteRequested(object sender, EventArgs e)
        {
            if (CheckForNullTemplateItemsRepository())
                return;

            if (!(sender is EditableAgeCategoryTemplateItem ageCategoryTemplateItemToDelete))
            {
                DisplayInvalidDeleteMessage(AgeCategoryTemplateItemName);
                return;
            }

            try
            {
                await _ageCategoryTemplateItemsRepository.RemoveAsync(ageCategoryTemplateItemToDelete.DbEntity);
            }
            catch (Exception)
            {
                MessageBox.Show("Nie udało się usunąć kategorii");
            }
        }

        private async void OnAddAgeCategoryTemplateItemAsync()
        {
            if (CheckForNullTemplateItemsRepository())
                return;

            if (String.IsNullOrWhiteSpace(NewAgeCategoryTemplateItem.Name) ||
                String.IsNullOrWhiteSpace(NewAgeCategoryTemplateItem.YearFrom.ToString()) ||
                String.IsNullOrWhiteSpace(NewAgeCategoryTemplateItem.YearTo.ToString()) ||
                !int.TryParse(NewAgeCategoryTemplateItem.YearFrom.ToString(), out int result1) ||
                !int.TryParse(NewAgeCategoryTemplateItem.YearFrom.ToString(), out int result2))
            {
                MessageBox.Show("Kategorie i lata graniczne nie mogę być puste");
                return;
            }
            AddAgeCategoryTemplateItemToGui(NewAgeCategoryTemplateItem);

            // Here we need to add it to the selected template

            await _ageCategoryTemplateItemsRepository.AddAsync(NewAgeCategoryTemplateItem.DbEntity);
            NewAgeCategoryTemplateItem = new EditableAgeCategoryTemplateItem();
        }



        private async void OnRepeatAgeCategoryTemplateItemsForWomen()
        {
            if (CheckForNullTemplateItemsRepository())
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

                await _ageCategoryTemplateItemsRepository.AddRangeAsync(repeatedAgeCategoryTemplateItemsForWomen.Select(category => category.DbEntity));

                await DownloadAgeCategoryTemplateItemsFromDatabase(clearDisplayedTemplateItemsBefore: true);
            }
        }

        private bool CheckForNullTemplateItemsRepository()
        {
            if (_ageCategoryTemplateItemsRepository == null)
            {
                MessageBox.Show("Szablon kategorii wiekowych nie został wybrany");
                return true;
            }
            return false;
        }

        private async void OnClearAgeCategoryTemplates()
        {
            if (CheckForNullTemplateItemsRepository())
                return;

            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz usunąć wszystkie kategorie wiekowe?") == MessageBoxResult.Yes)
            {
                try
                {
                    await _ageCategoryTemplateItemsRepository.RemoveRangeAsync(AgeCategoryTemplateItems.Select(editable => editable.DbEntity));
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


        #region Properties and events

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

        private EditableAgeCategoryTemplate _selectedAgeCategoryCollection = new EditableAgeCategoryTemplate();
        public EditableAgeCategoryTemplate SelectedAgeCategoryCollection
        {
            get { return _selectedAgeCategoryCollection; }
            set { SetProperty(ref _selectedAgeCategoryCollection, value); }
        }


        private ObservableCollection<EditableAgeCategoryTemplateItem> _ageCategoryTemplateItems = new ObservableCollection<EditableAgeCategoryTemplateItem>();
        public ObservableCollection<EditableAgeCategoryTemplateItem> AgeCategoryTemplateItems
        {
            get { return _ageCategoryTemplateItems; }
            set { SetProperty(ref _ageCategoryTemplateItems, value); }
        }

        private EditableAgeCategoryTemplateItem _newAgeCategoryTemplateItem = new EditableAgeCategoryTemplateItem();
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



        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand AddAgeCategoryCollectionCmd { get; private set; }
        public RelayCommand AddAgeCategoryTemplateCmd { get; private set; }
        //public RelayCommand DeleteAgeCategoryCollectionCmd { get; private set; }
        //public RelayCommand DeleteAgeCategoryTemplateCmd { get; private set; }
        public RelayCommand RepeatAgeCategoryTemplateItemsForWomenCmd { get; private set; }
        public RelayCommand ClearAgeCategoryTemplatesCmd { get; private set; }

        #endregion
    }
}
