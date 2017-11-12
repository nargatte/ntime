using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Entities
{
    class EditablePlayer : BindableBase
    {

        private int _startNumber;
        public int StartNumber
        {
            get { return _startNumber; }
            set { SetProperty(ref _startNumber, value); }
        }


        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }


        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }


        private string _distanceName;
        public string DistanceName
        {
            get { return _distanceName; }
            set { SetProperty(ref _distanceName, value); }
        }


        private DateTime _birthDate;
        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { SetProperty(ref _birthDate, value); }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { SetProperty(ref _startTime, value); }
        }


        private string _team;
        public string Team
        {
            get { return _team; }
            set { SetProperty(ref _team, value); }
        }


        private string _extraPlayerInfo;
        public string ExtraPlayerInfo
        {
            get { return _extraPlayerInfo; }
            set { SetProperty(ref _extraPlayerInfo, value); }
        }


        private string _phoneNumber;
        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { SetProperty(ref _phoneNumber, value); }
        }


        private bool _isMale;
        public bool IsMale
        {
            get { return _isMale; }
            set { SetProperty(ref _isMale, value); }
        }


        private string _fullCategory;
        public string FullCategory
        {
            get { return _fullCategory; }
            set { SetProperty(ref _fullCategory, value); }
        }

    }
}
