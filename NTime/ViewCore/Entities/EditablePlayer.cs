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
            DefinedDistances = new ObservableCollection<EditableDistance>();
            DefinedSubcategory = new ObservableCollection<EditableSubcategory>();
        }

        public EditablePlayer(IEditableCompetition currentComptetition, ICollection<EditableDistance> distances,
            ICollection<EditableSubcategory> subcategories, Player dbPlayer) : this(currentComptetition)
        {
            DbEntity = dbPlayer;
            DefinedDistances = new ObservableCollection<EditableDistance>(distances);
            DefinedSubcategory = new ObservableCollection<EditableSubcategory>(subcategories);
        }

        public int StartNumber
        {
            get { return DbEntity.StartNumber; }
            set
            {
                DbEntity.StartNumber = SetProperty(DbEntity.StartNumber, value);
                OnUpdateRequested();
            }
        }

        public string FirstName
        {
            get { return DbEntity.FirstName; }
            set
            {
                DbEntity.FirstName = SetProperty(DbEntity.FirstName, value);
                OnUpdateRequested();
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

        public int CompetitionId
        {
            get { return DbEntity.CompetitionId; }
            set
            {
                DbEntity.CompetitionId = SetProperty(DbEntity.CompetitionId, value);
                OnUpdateRequested();
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
                if (value != null)
                    if (value != null && !String.IsNullOrWhiteSpace(value.Name))
                    {
                        DbEntity.Distance = SetProperty(DbEntity.Distance, value.DbEntity);
                        OnUpdateRequested();
                        OnPropertyChanged(nameof(FullCategory));
                    }
            }
        }

        public EditableSubcategory Subcategories
        {
            get
            {
                var temp = DefinedSubcategory.FirstOrDefault(info => Equals(info.DbEntity, DbEntity.Subcategory));
                return temp;
            }
            set
            {
                if (value != null)
                    if (!String.IsNullOrWhiteSpace(value.Name))
                    {
                        DbEntity.Subcategory = SetProperty(DbEntity.Subcategory, value.DbEntity);
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
                DbEntity.BirthDate = SetProperty(DbEntity.BirthDate, value);
                OnUpdateRequested();
            }
        }

        public string StartTime
        {
            get { return DbEntity.StartTime.GetValueOrDefault().ConvertToString(); }
            set
            {
                if (value != null)
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

        public bool IsPaidUp
        {
            get { return DbEntity.IsPaidUp; }
            set
            {
                DbEntity.IsPaidUp = SetProperty(DbEntity.IsPaidUp, value);
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


        private ObservableCollection<EditableSubcategory> _definedSubcategory;
        public ObservableCollection<EditableSubcategory> DefinedSubcategory
        {
            get { return _definedSubcategory; }
            set { SetProperty(ref _definedSubcategory, value); }
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
