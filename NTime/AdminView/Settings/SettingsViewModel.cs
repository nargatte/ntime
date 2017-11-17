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
    class SettingsViewModel : TabItemViewModel, ViewCore.Entities.ISwitchableViewModel
    {
        CompetitionRepository repository = new CompetitionRepository(new ContextProvider());
        public SettingsViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            this.CurrentCompetition = currentCompetition;
            TabTitle = "Ustawienia";
            SaveChangesCmd = new RelayCommand(OnSaveChanges);
        }


        //private Entities.EditableCompetition _currentCompetition;
        public ViewCore.Entities.IEditableCompetition CurrentCompetition
        {
            get { return _currentCompetition; }
            set { SetProperty(ref _currentCompetition, value);
                //OnPropertyChanged("Name");
                //OnPropertyChanged("CurrentCompetition.Name");
            }
        }

        private void OnSaveChanges()
        {
            repository.UpdateAsync(CurrentCompetition.Competition).ContinueWith( t => 
            MessageBox.Show("Zmiany zostały zapisane")
            );
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        public RelayCommand SaveChangesCmd { get; private set; }
        public event Action AddCompetitionRequested = delegate { };
    }
}
