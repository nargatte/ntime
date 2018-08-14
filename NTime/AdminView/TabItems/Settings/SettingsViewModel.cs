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
using ViewCore;
using ViewCore.Entities;

namespace AdminView.Settings
{
    class SettingsViewModel : TabItemViewModel
    {
        CompetitionRepository repository = new CompetitionRepository(new ContextProvider());
        public SettingsViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            TabTitle = "Ustawienia";
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            SaveChangesCmd = new RelayCommand(OnSaveChangesAsync);
            RemoveCompetitionCmd = new RelayCommand(OnRemoveCompetition);
            AddExtraHeaderCmd = new RelayCommand(OnAddExtraHeader);
            PrepareNewExtraHeader();
        }

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



        private EditableHeaderPermutationPair _newExtraHeader;
        public EditableHeaderPermutationPair NewExtraHeader // This might not worked as supposed
        {
            get { return _newExtraHeader; }
            set { SetProperty(ref _newExtraHeader, value); }
        }

        private void OnViewLoadedAsync()
        {
            DownloadExtraHeaders();
        }

        private void DownloadExtraHeaders()
        {
            ExtraHeaders.Clear();
            var downloadedExtraHeaders = _competitionRepository.GetHeaderPermutationPairs(CurrentCompetition.DbEntity.ExtraDataHeaders).ToList();
            foreach (var header in downloadedExtraHeaders)
            {
                ExtraHeaders.Add(PrepareNewExtraHeader(header));
            }
        }


        private async void OnSaveChangesAsync()
        {
            if(ExtraHeaders.Any(header => !IsHeaderInputCorrect(header)))
            {
                MessageBox.Show("Nazwy kolumn nie mogą być puste");
                return;
            }
            await CompetitionRepositoryHelper.ModifyExtraDataHeaders(
                ExtraHeaders.Select(extraHeader => extraHeader.DbEntity).ToArray(), CurrentCompetition.DbEntity
            );
            MessageBox.Show("Zmiany zostały zapisane");
            DownloadExtraHeaders();

            //await repository.UpdateAsync(_currentCompetition.DbEntity).ContinueWith(t =>
            //    MessageBox.Show("Zmiany zostały zapisane")
            //);
        }

        private void OnAddExtraHeader()
        {
            if (!IsHeaderInputCorrect(NewExtraHeader))
            {
                MessageBox.Show("Nagłówek nie może być pusty");
                return;
            }
            ExtraHeaders.Add(PrepareNewExtraHeader(NewExtraHeader.DbEntity));
            PrepareNewExtraHeader();
        }


        private bool IsHeaderInputCorrect(EditableHeaderPermutationPair editableHeader)
        {
            if (String.IsNullOrWhiteSpace(editableHeader.HeaderName))
                return false;
            else
                return true;
        }

        private void PrepareNewExtraHeader()
        {
            NewExtraHeader = new EditableHeaderPermutationPair(CurrentCompetition);
            NewExtraHeader.PermutationElement = -1;
        }

        private void OnRemoveCompetition()
        {
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz usunąć te zawody? Zmiana jest nieodwracalna!!!",
                $"",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                repository.RemoveAsync(CurrentCompetition.DbEntity);
                CompetitionRemoved?.Invoke();
            }
        }

        private EditableHeaderPermutationPair PrepareNewExtraHeader(HeaderPermutationPair dbEntity)
        {
            var editableHeader = new EditableHeaderPermutationPair(CurrentCompetition) { DbEntity = dbEntity };
            editableHeader.DeleteRequested += EditableHeader_DeleteRequested;
            editableHeader.MoveUpRequested += EditableHeader_MoveUpRequested;
            editableHeader.MoveDownRequested += EditableHeader_MoveDownRequested;
            return editableHeader;
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
            if (editableHeader == ExtraHeaders.Last() )
                return;
            int currentIndex = ExtraHeaders.IndexOf(editableHeader);
            ExtraHeaders.Move(currentIndex, currentIndex + 1);
        }


        public RelayCommand SaveChangesCmd { get; private set; }
        public RelayCommand RemoveCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand AddExtraHeaderCmd { get; private set; }

        public event Action CompetitionRemoved = delegate { };
    }
}
