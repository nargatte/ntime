using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using BaseCore.DataBase.RepositoriesHelpers;
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
            _newExtraHeader = new EditableHeaderPermutationPair(currentCompetition);
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
            downloadedExtraHeaders.ForEach(item => ExtraHeaders.Add(new EditableHeaderPermutationPair(CurrentCompetition) { DbEntity = item }));
        }

        private async void OnSaveChangesAsync()
        {
            await CompetitionRepositoryHelper.ModifyExtraDataHeaders(
                ExtraHeaders.Select(extraHeader => extraHeader.DbEntity).ToArray(), CurrentCompetition.DbEntity
            ).ContinueWith(t => MessageBox.Show("Zmiany zostały zapisane"));
            //await repository.UpdateAsync(_currentCompetition.DbEntity).ContinueWith(t =>
            //    MessageBox.Show("Zmiany zostały zapisane")
            //);
        }

        private void OnAddExtraHeader()
        {
            ExtraHeaders.Add(NewExtraHeader);
            ClearNewExtraHeader();
        }

        private void ClearNewExtraHeader()
        {
            NewExtraHeader = new EditableHeaderPermutationPair(CurrentCompetition);
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

        public RelayCommand SaveChangesCmd { get; private set; }
        public RelayCommand RemoveCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand AddExtraHeaderCmd { get; private set; }

        public event Action CompetitionRemoved = delegate { };
    }
}
