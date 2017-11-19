using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.TimesProcess;
using BaseCore.DataBase;
using MvvmHelper;

namespace ViewCore.Entities
{
    public class EditablePlayer : EditableBaseClass<Player>
    {
        public EditablePlayer(IEditableCompetition currentComptetition): base(currentComptetition) { }
        public int StartNumber
        {
            get { return DbEntity.StartNumber; }
            set { DbEntity.StartNumber = SetProperty(DbEntity.StartNumber, value); }
        }

        public string FirstName
        {
            get { return DbEntity.FirstName; }
            set { DbEntity.FirstName = SetProperty(DbEntity.FirstName, value); }
        }


        public string LastName
        {
            get { return DbEntity.LastName; }
            set { DbEntity.LastName = SetProperty(DbEntity.LastName, value); }
        }

        //TODO Grzesiek
        public Distance Distance
        {
            get { return DbEntity.Distance; }
            set { DbEntity.Distance = SetProperty(DbEntity.Distance, value); }
        }


        public DateTime BirthDate
        {
            get { return DbEntity.BirthDate; }
            set { DbEntity.BirthDate = SetProperty(DbEntity.BirthDate, value); }
        }


        public DateTime? StartTime
        {
            get { return DbEntity.StartTime; }
            set { DbEntity.StartTime = SetProperty(DbEntity.StartTime, value); }
        }


        public string Team
        {
            get { return DbEntity.Team; }
            set { DbEntity.Team = SetProperty(DbEntity.Team, value); }
        }


        public ExtraPlayerInfo ExtraPlayerInfo
        {
            get { return DbEntity.ExtraPlayerInfo; }
            set { DbEntity.ExtraPlayerInfo = SetProperty(DbEntity.ExtraPlayerInfo, value); }
        }

        public string PhoneNumber
        {
            get { return DbEntity.PhoneNumber; }
            set { DbEntity.PhoneNumber = SetProperty(DbEntity.PhoneNumber, value); }
        }

        public bool IsMale
        {
            get { return DbEntity.IsMale; }
            set { DbEntity.IsMale = SetProperty(DbEntity.IsMale, value); }
        }

        public string FullCategory
        {
            get { return DbEntity.FullCategory; }
            set { DbEntity.FullCategory = SetProperty(DbEntity.FullCategory, value); }
        }

        public int DistancePlaceNumber
        {
            get { return DbEntity.DistancePlaceNumber; }
            set { DbEntity.DistancePlaceNumber = SetProperty(DbEntity.DistancePlaceNumber, value); }
        }

        public int CategoryPlaceNumber
        {
            get { return DbEntity.CategoryPlaceNumber; }
            set { DbEntity.CategoryPlaceNumber = SetProperty(DbEntity.CategoryPlaceNumber, value); }
        }


        public int LapsCount
        {
            get { return DbEntity.LapsCount; }
            set { DbEntity.LapsCount = SetProperty(DbEntity.LapsCount, value); }
        }

        public string Time
        {
            get { return DbEntity.Time.ToDateTime().ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                    DbEntity.Time = SetProperty(DbEntity.Time, dateTime.ToDecimal());
            }
        }


    }
}
