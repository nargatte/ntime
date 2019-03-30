using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using BaseCore.DataBase.RepositoriesHelpers;
using BaseCore.Models;
using MvvmHelper;
using NTime.Application.Services;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Helpers;

namespace AdminView.Settings
{
    class SettingsViewModel : TabItemViewModel
    {
        private readonly IExtraColumnService _extraColumnService;

        private List<ExtraColumn> _originalExtraColumns = new List<ExtraColumn>();

        public SettingsViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Ustawienia";
            _competitionRepository = new CompetitionRepository(new ContextProvider());
            _extraColumnService = new ExtraColumnService(currentCompetition.DbEntity);

            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            SaveChangesCmd = new RelayCommand(OnSaveChangesAsync);
            RemoveCompetitionCmd = new RelayCommand(OnRemoveCompetition);
            AddExtraHeaderCmd = new RelayCommand(OnAddExtraHeader);
            AddExtraColumnCmd = new RelayCommand(OnAddExtraColumn);
            PrepareNewExtraHeader();
            PrepareNewExtraColumn();
        }


        #region Properties
        public IEditableCompetition CurrentCompetition
        {
            get { return _currentCompetition; }
            set { SetProperty(ref _currentCompetition, value); }
        }


        private ObservableCollection<EditableHeaderPermutationPair> _extraHeaders = new ObservableCollection<EditableHeaderPermutationPair>();
        public ObservableCollection<EditableHeaderPermutationPair> ExtraHeaders
        {
            get { return _extraHeaders; }
            set { SetProperty(ref _extraHeaders, value); }
        }


        private ObservableCollection<EditableExtraColumn> _extraColumns = new ObservableCollection<EditableExtraColumn>();
        public ObservableCollection<EditableExtraColumn> ExtraColumns
        {
            get { return _extraColumns; }
            set { SetProperty(ref _extraColumns, value); }
        }

        public List<EditableHeaderPermutationPair> OriginalExtraHeaders { get; set; } = new List<EditableHeaderPermutationPair>();



        private EditableHeaderPermutationPair _newExtraHeader;
        public EditableHeaderPermutationPair NewExtraHeader // This might not worked as supposed
        {
            get { return _newExtraHeader; }
            set { SetProperty(ref _newExtraHeader, value); }
        }


        private EditableExtraColumn _newExtraColumn;
        public EditableExtraColumn NewExtraColumn
        {
            get { return _newExtraColumn; }
            set { SetProperty(ref _newExtraColumn, value); }
        }

        #endregion

        private async void OnViewLoadedAsync()
        {
            DownloadExtraHeaders();
            await DownloadExtraColumnsAsync();
        }

        private void DownloadExtraHeaders()
        {
            ExtraHeaders.Clear();
            var downloadedExtraHeaders = _competitionRepository.GetHeaderPermutationPairs(CurrentCompetition.DbEntity.ExtraDataHeaders).ToList();
            foreach (var header in downloadedExtraHeaders)
            {
                ExtraHeaders.Add(PrepareNewExtraHeader(header));
            }
            OriginalExtraHeaders = new List<EditableHeaderPermutationPair>(ExtraHeaders);
        }

        private async Task DownloadExtraColumnsAsync()
        {
            var extraColumns = await _extraColumnService.GetExtraColumnsForCompetition();
            _originalExtraColumns = new List<ExtraColumn>(extraColumns.Select(column => new ExtraColumn(column)));
            ExtraColumns = new ObservableCollection<EditableExtraColumn>(
                extraColumns.Select(dbColumn => GetNewExtraColumnWithEvents(dbColumn)));
        }




        private async void OnSaveChangesAsync()
        {
            if (ExtraHeaders.Any(header => !IsHeaderInputCorrect(header.HeaderName)))
            {
                MessageBox.Show("Nazwy kolumn nie mogą być puste");
                return;
            }
            try
            {
                var tasks = new List<Task>();
                var updatedDbColumns = _extraColumnService.GetExtraColumnsWithSortIndices(ExtraColumns.Select(column => column.DbEntity));
                if (_extraColumnService.ExtraColumnsChanged(_originalExtraColumns, updatedDbColumns))
                    tasks.Add(_extraColumnService.UpdateExtraColumns(updatedDbColumns));
                tasks.Add(base._competitionRepository.UpdateAsync(CurrentCompetition.DbEntity));
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Zmian nie udało się zapisać. Powód: {ex.Message}");
            }
            MessageBox.Show("Zmiany zostały zapisane");
            DownloadExtraHeaders();
            await DownloadExtraColumnsAsync();
        }

        private bool ExtraHeadersChanged()
        {
            if (OriginalExtraHeaders.Count != ExtraHeaders.Count)
                return true;
            var originalEnumerator = OriginalExtraHeaders.GetEnumerator();
            bool headersChanged = false;
            foreach (var header in ExtraHeaders)
            {
                if (!originalEnumerator.MoveNext())
                    break;
                var original = originalEnumerator.Current;
                if (original.PermutationElement != header.PermutationElement)
                {
                    headersChanged = true;
                    break;
                }
            }
            return headersChanged;
        }

        private void OnAddExtraHeader()
        {
            if (!IsHeaderInputCorrect(NewExtraHeader.HeaderName))
            {
                MessageBox.Show("Nagłówek nie może być pusty");
                return;
            }
            ExtraHeaders.Add(PrepareNewExtraHeader(NewExtraHeader.DbEntity));
            PrepareNewExtraHeader();
        }


        private async void OnAddExtraColumn()
        {
            if (!IsHeaderInputCorrect(NewExtraColumn?.Title))
            {
                MessageBox.Show("Nagłówek nie może być pusty");
                return;
            }
            try
            {
                await _extraColumnService.AddExtraColumn(NewExtraColumn.DbEntity);
                MessageBox.Show("Kolumna została dodana do bazy");
                ExtraColumns.Add(GetNewExtraColumnWithEvents(NewExtraColumn.DbEntity));
                PrepareNewExtraColumn();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Dodawanie kolumny nie powiodło się. Powód: {ex.Message}");
                return;
            }
        }

        private bool IsHeaderInputCorrect(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return false;
            else
                return true;
        }



        private void PrepareNewExtraHeader()
        {
            NewExtraHeader = new EditableHeaderPermutationPair(CurrentCompetition)
            {
                PermutationElement = -1
            };
        }

        private EditableHeaderPermutationPair PrepareNewExtraHeader(HeaderPermutationPair dbEntity)
        {
            var editableHeader = new EditableHeaderPermutationPair(CurrentCompetition) { DbEntity = dbEntity };
            editableHeader.DeleteRequested += EditableHeader_DeleteRequested;
            editableHeader.MoveUpRequested += EditableHeader_MoveUpRequested;
            editableHeader.MoveDownRequested += EditableHeader_MoveDownRequested;
            return editableHeader;
        }

        private void PrepareNewExtraColumn()
        {
            var dbExtraColumn = new ExtraColumn();
            NewExtraColumn = GetNewExtraColumnWithEvents(dbExtraColumn);
        }

        private EditableExtraColumn GetNewExtraColumnWithEvents(ExtraColumn dbEntity)
        {
            var editableExtraColumn = new EditableExtraColumn(CurrentCompetition) { DbEntity = dbEntity };
            editableExtraColumn.DeleteRequested += EditableExtraColumn_DeleteRequested;
            editableExtraColumn.MoveUpRequested += EditableExtraColumn_MoveUpRequested;
            editableExtraColumn.MoveDownRequested += EditableExtraColumn_MoveDownRequested;
            return editableExtraColumn;
        }

        private async void EditableExtraColumn_DeleteRequested(object sender, EventArgs e)
        {
            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz trwale usunąć tę kolumnę? Kolumna zostanie także usunięta dla wszystkich zawodników!")
                == MessageBoxResult.Yes)
            {
                var editableExtraColumn = sender as EditableExtraColumn;
                if (editableExtraColumn == null)
                    return;
                try
                {
                    await _extraColumnService.RemoveExtraColumn(editableExtraColumn.DbEntity);
                    MessageBox.Show("Kolumna została usunięta");
                    ExtraColumns.Remove(editableExtraColumn);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Nie udało się usunąć kolumny. Powód: {ex.Message}");
                    return;
                }
            }
        }

        private void EditableExtraColumn_MoveUpRequested(object sender, EventArgs e)
        {
            var editableHeader = sender as EditableExtraColumn;
            if (editableHeader == null || editableHeader == ExtraColumns.First())
                return;
            int oldIndex = ExtraColumns.IndexOf(editableHeader);
            int newIndex = oldIndex - 1;
            ExtraColumns.Move(oldIndex, newIndex);
        }

        private void EditableExtraColumn_MoveDownRequested(object sender, EventArgs e)
        {
            var editableHeader = sender as EditableExtraColumn;
            if (editableHeader == null || editableHeader == ExtraColumns.Last())
                return;
            int oldIndex = ExtraColumns.IndexOf(editableHeader);
            int newIndex = oldIndex + 1;
            ExtraColumns.Move(oldIndex, newIndex);
        }

        private void OnRemoveCompetition()
        {
            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz usunąć te zawody? Zmiana jest nieodwracalna!!!") == MessageBoxResult.Yes)
            {
                _competitionRepository.RemoveAsync(CurrentCompetition.DbEntity);
                CompetitionRemoved?.Invoke();
            }
        }


        private void EditableHeader_DeleteRequested(object sender, EventArgs e)
        {
            var editableHeader = sender as EditableHeaderPermutationPair;
            ExtraHeaders.Remove(editableHeader);
        }

        private void EditableHeader_MoveUpRequested(object sender, EventArgs e)
        {
            var editableHeader = sender as EditableHeaderPermutationPair;
            if (editableHeader == ExtraHeaders.First())
                return;
            int currentIndex = ExtraHeaders.IndexOf(editableHeader);
            ExtraHeaders.Move(currentIndex, currentIndex - 1);
        }

        private void EditableHeader_MoveDownRequested(object sender, EventArgs e)
        {
            var editableHeader = sender as EditableHeaderPermutationPair;
            if (editableHeader == ExtraHeaders.Last())
                return;
            int currentIndex = ExtraHeaders.IndexOf(editableHeader);
            ExtraHeaders.Move(currentIndex, currentIndex + 1);
        }


        public RelayCommand SaveChangesCmd { get; private set; }
        public RelayCommand RemoveCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand AddExtraHeaderCmd { get; private set; }
        public RelayCommand AddExtraColumnCmd { get; private set; }

        public event Action CompetitionRemoved = delegate { };
    }
}
