using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Settings
{
    class SettingsViewModel : TabItemViewModel, IViewModel
    {
        public SettingsViewModel()
        {
            TabTitle = "Ustawienia";
            SaveChangesCmd = new RelayCommand(OnSaveChanges);
        }


        private Entities.EditableCompetition _currentCompetition = new Entities.EditableCompetition();
        public Entities.EditableCompetition CurrentCompetition
        {
            get { return _currentCompetition; }
            set { SetProperty(ref _currentCompetition, value);
                OnPropertyChanged("Name");
                OnPropertyChanged("CurrentCompetition.Name");
            }
        }

        private void OnSaveChanges()
        {
            CurrentCompetition.Name += "New ";
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        public RelayCommand SaveChangesCmd { get; private set; }
        public event Action AddCompetitionRequested = delegate { };
    }
}
