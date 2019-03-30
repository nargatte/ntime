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
        CompetitionRepository _repository;
        private readonly IExtraColumnService _extraColumnService;

        public SettingsViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Ustawienia";
            _repository = new CompetitionRepository(new ContextProvider());
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
        public ViewCore.Entities.IEditableCompetition CurrentCompetition
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
            ExtraColumns = new ObservableCollection<EditableExtraColumn>(
                extraColumns.Select(dbColumn => PrepareNewExtraColumn(dbColumn)));
        }

        


        private async void OnSaveChangesAsync()
        {
            if (ExtraHeaders.Any(header => !IsHeaderInputCorrect(header.HeaderName)))
            {
                MessageBox.Show("Nazwy kolumn nie mogą być puste");
                return;
            }
            if (ExtraHeadersChanged())
                await CompetitionRepositoryHelper.ModifyExtraDataHeaders(
                    ExtraHeaders.Select(extraHeader => extraHeader.DbEntity).ToArray(), CurrentCompetition.DbEntity
                );
            else
                await _competitionRepository.UpdateAsync(CurrentCompetition.DbEntity);
            MessageBox.Show("Zmiany zostały zapisane");
            DownloadExtraHeaders();

            //await repository.UpdateAsync(_currentCompetition.DbEntity).ContinueWith(t =>
            //    MessageBox.Show("Zmiany zostały zapisane")
            //);
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
                if(original.PermutationElement != header.PermutationElement)
                {
                    headersChanged = true;
                    break;
                }
            }
            return headersChanged;
        }

        private void OnAddExtraHeader()
        {
            if (!IsHeaderInputCorrect(NewExtraColumn.Title))
            {
                MessageBox.Show("Nagłówek nie może być pusty");
                return;
            }
            ExtraHeaders.Add(PrepareNewExtraHeader(NewExtraHeader.DbEntity));
            PrepareNewExtraHeader();
        }


        private void OnAddExtraColumn()
        {
            if (!IsHeaderInputCorrect(NewExtraColumn.Title))
            {
                MessageBox.Show("Nagłówek nie może być pusty");
                return;
            }
            ExtraColumns.Add(PrepareNewExtraColumn(NewExtraColumn.DbEntity));
            PrepareNewExtraColumn();
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
            NewExtraColumn = new EditableExtraColumn(CurrentCompetition);
        }

        private EditableExtraColumn PrepareNewExtraColumn(ExtraColumn dbEntity)
        {
            var editableExtraColumn = new EditableExtraColumn(CurrentCompetition) { DbEntity = dbEntity };
            editableExtraColumn.DeleteRequested += EditableHeader_DeleteRequested;
            editableExtraColumn.MoveUpRequested += EditableHeader_MoveUpRequested;
            editableExtraColumn.MoveDownRequested += EditableHeader_MoveDownRequested;
            return editableExtraColumn;
        }

        private void OnRemoveCompetition()
        {
            if (MessageBoxHelper.DisplayYesNo("Czy na pewno chcesz usunąć te zawody? Zmiana jest nieodwracalna!!!") == MessageBoxResult.Yes)
            {
                _repository.RemoveAsync(CurrentCompetition.DbEntity);
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
