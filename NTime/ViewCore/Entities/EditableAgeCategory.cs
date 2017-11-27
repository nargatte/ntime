using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableAgeCategory : EditableBaseClass<AgeCategory>
    {
        public EditableAgeCategory(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
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


        public int YearFrom
        {
            get { return DbEntity.YearFrom; }
            set
            {
                DbEntity.YearFrom = SetProperty(DbEntity.YearFrom, value);
                OnUpdateRequested();
            }
        }

        public int YearTo
        {
            get { return DbEntity.YearTo; }
            set
            {
                DbEntity.YearTo = SetProperty(DbEntity.YearTo, value);
                OnUpdateRequested();
            }
        }

        private void OnDeleteCategory()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public RelayCommand DeleteCategoryCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler UpdateRequested = delegate { };
        protected void OnUpdateRequested()
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }


    }
}
