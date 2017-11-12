using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Entities
{
    class EditableCompetition : BindableBase
    {
        private string _name = "Old Text";
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value);
                OnPropertyChanged("Name");
            }
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
    }
}
