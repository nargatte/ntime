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


        private string _name = String.Empty;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }


        private string _city = String.Empty;
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }


        private DateTime _eventDate = new DateTime();
        public DateTime EventDate
        {
            get { return _eventDate; }
            set { SetProperty(ref _eventDate, value); }
        }


        private string _organiser = String.Empty;
        public string Organiser
        {
            get { return _organiser; }
            set { SetProperty(ref _organiser, value); }
        }


        private string _description = String.Empty;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }


        private string _link = String.Empty;
        public string Link
        {
            get { return _link; }
            set { SetProperty(ref _link, value); }
        }


        private void OnSaveChanges()
        {
            throw new NotImplementedException();
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        public RelayCommand SaveChangesCmd { get; private set; }
        public event Action AddCompetitionRequested = delegate { };
    }
}
