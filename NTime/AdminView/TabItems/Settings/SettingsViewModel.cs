using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;

namespace AdminView.Settings
{
    class SettingsViewModel : TabItemViewModel
    {
        CompetitionRepository repository = new CompetitionRepository(new ContextProvider());
        public SettingsViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            //this.CurrentCompetition = currentCompetition;
            TabTitle = "Ustawienia";
            SaveChangesCmd = new RelayCommand(OnSaveChanges);
            RemoveCompetitionCmd = new RelayCommand(OnRemoveCompetition);
        }

        public ViewCore.Entities.IEditableCompetition CurrentCompetition
        {
            get { return _currentCompetition; }
            set { SetProperty(ref _currentCompetition, value); }
        }

        private void OnSaveChanges()
        {
            repository.UpdateAsync(_currentCompetition.DbEntity).ContinueWith(t =>
           MessageBox.Show("Zmiany zostały zapisane")
            );
        }

        private void OnRemoveCompetition()
        {
            MessageBoxResult result = MessageBox.Show(
             $"Czy na pewno chcesz usunąć te zawody. Zmiana jest nieodwracalna?",
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

        public event Action AddCompetitionRequested = delegate { }; //Probably unnecessary
        public event Action CompetitionRemoved = delegate { };
    }
}
