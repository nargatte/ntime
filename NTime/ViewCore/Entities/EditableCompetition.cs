using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Entities
{
    public class EditableCompetition : BindableBase
    {
        private BaseCore.DataBase.Competition _competition = new BaseCore.DataBase.Competition();
        public BaseCore.DataBase.Competition Competition
        {
            get { return _competition; }
            set { _competition = value; }
        }


        public string Name
        {
            get { return Competition.Name; }
            set { Competition.Name = SetProperty(Competition.Name, value); }
        }


        private string _nam2;
        public string Name2
        {
            get { return _nam2; }
            set { SetProperty(ref _nam2, value); }
        }

        //private string _name = "Old Text";
        //public string Name
        //{
        //    get { return _name; }
        //    set { SetProperty(ref _name, value);
        //        OnPropertyChanged("Name");
        //    }
        //}


        public string City
        {
            get { return Competition.City; }
            set { Competition.City = SetProperty(Competition.City, value); }
        }


        public DateTime EventDate
        {
            get { return Competition.EventDate; }
            set { Competition.EventDate = SetProperty(Competition.EventDate, value); }
        }


        public string Organiser
        {
            get { return Competition.Organiser; }
            set { Competition.Organiser = SetProperty(Competition.Organiser, value); }
        }


        public string Description
        {
            get { return Competition.Description; }
            set { Competition.Description = SetProperty(Competition.Description, value); }
        }


        public string Link
        {
            get { return Competition.Link; }
            set { Competition.Link = SetProperty(Competition.Link, value); }
        }
    }
}
