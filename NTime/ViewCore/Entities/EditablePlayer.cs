using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.TimesProcess;
using BaseCore.DataBase;
using MvvmHelper;
using System.Diagnostics;
using System.Threading;

namespace ViewCore.Entities
{
    public class EditablePlayer : EditableBaseClass<Player>, ICloneable
    {

        public EditablePlayer(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            StartTime.TryConvertToDateTime(out DateTime startTimeDateTime);
            if (StartTime == null || startTimeDateTime < new DateTime(2000, 1, 1))
            {
                StartTime = DateTime.Today.ConvertToString();
            }

            if (BirthDate == null || BirthDate < new DateTime(2000, 1, 1))
                BirthDate = DateTime.Today;
        }

        public EditablePlayer(IEditableCompetition currentComptetition, ICollection<EditableDistance> distances,
            ICollection<EditableExtraPlayerInfo> extraPlayerInfos, Player dbPlayer) : this(currentComptetition)
        {
            DbEntity = dbPlayer;
            DefinedDistances = new ObservableCollection<EditableDistance>(distances);
            DefinedExtraPlayerInfo = new ObservableCollection<EditableExtraPlayerInfo>(extraPlayerInfos);
        }

        public int StartNumber
        {
            get { return DbEntity.StartNumber; }
            set
            {
                OnUpdateRequested();
                DbEntity.StartNumber = SetProperty(DbEntity.StartNumber, value);
            }
        }

        public string FirstName
        {
            get { return DbEntity.FirstName; }
            set
            {
                OnUpdateRequested();
                DbEntity.FirstName = SetProperty(DbEntity.FirstName, value);
            }
        }


        public string LastName
        {
            get { return DbEntity.LastName; }
            set
            {
                OnUpdateRequested();
                DbEntity.LastName = SetProperty(DbEntity.LastName, value);
            }
        }

        //Some notifiers might be necessary
        public EditableDistance Distance
        {
            get
            {
                var temp = DefinedDistances.FirstOrDefault(dist => Equals(dist.DbEntity, DbEntity.Distance));
                return temp;
            }
            set
            {
                if(!String.IsNullOrWhiteSpace(value.Name))
                {
                    DbEntity.Distance = SetProperty(DbEntity.Distance, value.DbEntity);
                    OnUpdateRequested();
                    OnPropertyChanged(nameof(FullCategory));
                }
            }
        }

        public EditableExtraPlayerInfo ExtraPlayerInfo
        {
            get
            {
                var temp = DefinedExtraPlayerInfo.FirstOrDefault(info => Equals(info.DbEntity, DbEntity.ExtraPlayerInfo));
                return temp;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value.Name))
                {
                    DbEntity.ExtraPlayerInfo = SetProperty(DbEntity.ExtraPlayerInfo, value.DbEntity);
                    OnUpdateRequested();
                    OnPropertyChanged(nameof(FullCategory));
                }
            }
        }

        public DateTime BirthDate
        {
            get { return DbEntity.BirthDate; }
            set
            {
                OnUpdateRequested();
                DbEntity.BirthDate = SetProperty(DbEntity.BirthDate, value);
            }
        }


        //public DateTime? StartTime
        //{
        //    get { return DbEntity.StartTime; }
        //    set
        //    {
        //        DbEntity.StartTime = SetProperty(DbEntity.StartTime, value);
        //        OnUpdateRequested();
        //    }
        //}

        public string StartTime
        {
            get { return DbEntity.StartTime.GetValueOrDefault().ConvertToString(); }
            set
            {
                if (value.TryConvertToDateTime(out DateTime dateTime))
                {
                    DbEntity.StartTime = SetProperty(DbEntity.StartTime, dateTime);
                    OnUpdateRequested();
                }
            }
        }

        internal void UpdateFullCategoryDisplay()
        {
            OnPropertyChanged(nameof(FullCategory));
        }

        public string Team
        {
            get { return DbEntity.Team; }
            set
            {
                DbEntity.Team = SetProperty(DbEntity.Team, value);
                OnUpdateRequested();
            }
        }



        public string PhoneNumber
        {
            get { return DbEntity.PhoneNumber; }
            set
            {
                DbEntity.PhoneNumber = SetProperty(DbEntity.PhoneNumber, value);
                OnUpdateRequested();
            }
        }

        public bool IsMale
        {
            get { return DbEntity.IsMale; }
            set
            {
                DbEntity.IsMale = SetProperty(DbEntity.IsMale, value);
                OnUpdateRequested();
            }
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

        private ObservableCollection<EditableDistance> _definedDistances;
        public ObservableCollection<EditableDistance> DefinedDistances
        {
            get { return _definedDistances; }
            set { SetProperty(ref _definedDistances, value); }
        }


        private ObservableCollection<EditableExtraPlayerInfo> _definedExtraPlayerInfo;
        public ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo
        {
            get { return _definedExtraPlayerInfo; }
            set { SetProperty(ref _definedExtraPlayerInfo, value); }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        protected void OnUpdateRequested()
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler UpdateRequested = delegate { };
    }
}
