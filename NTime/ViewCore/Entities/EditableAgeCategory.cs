using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableAgeCategory : EditableCompetitionItemBase<AgeCategory>
    {
        public EditableAgeCategory(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
            Male = true;
        }

        public EditableAgeCategory(EditableAgeCategory editableAgeCategory) : base(editableAgeCategory._currentCompetition) 
        {
            Name = editableAgeCategory.Name;
            YearFrom = editableAgeCategory.YearFrom;
            YearTo = editableAgeCategory.YearTo;
            Male = editableAgeCategory.Male;
            DbEntity.CompetitionId = editableAgeCategory.DbEntity.CompetitionId;
            //DeleteCategoryCmd = editableAgeCategory.DeleteCategoryCmd;
            //DeleteRequested = editableAgeCategory.DeleteRequested;
            //UpdateRequested = editableAgeCategory.UpdateRequested;
        }

        public string Name
        {
            get { return DbEntity.Name; }
            set
            {
                DbEntity.Name = SetProperty(DbEntity.Name, value);
                OnUpdateRequested();
            }
        }


        public int? YearFrom
        {
            get { return DbEntity.YearFrom != 0 ? DbEntity.YearFrom : (int?)null; }
            set
            {
                if (value.HasValue)
                {
                    DbEntity.YearFrom = SetProperty(DbEntity.YearFrom, value.Value);
                    OnUpdateRequested();
                }
            }
        }

        public int? YearTo
        {
            get { return DbEntity.YearTo != 0 ? DbEntity.YearTo : (int?)null; }
            set
            {
                if (value.HasValue)
                {
                    DbEntity.YearTo = SetProperty(DbEntity.YearTo, value.Value);
                    OnUpdateRequested();
                }
            }
        }


        public bool Male
        {
            get { return DbEntity.Male; }
            set
            {
                DbEntity.Male = SetProperty(DbEntity.Male, value);
                OnUpdateRequested();
            }
        }


        public RelayCommand DeleteCategoryCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler UpdateRequested = delegate { };

        private void OnDeleteCategory()
        {
            DeleteRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnUpdateRequested()
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }


    }
}
