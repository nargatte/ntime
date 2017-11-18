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
        public EditableAgeCategory()
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
        }

        public string Name
        {
            get { return DbEntity.Name; }
            set { DbEntity.Name = SetProperty(DbEntity.Name, value); }
        }


        private void OnDeleteCategory()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public int YearFrom
        {
            get { return DbEntity.YearFrom; }
            set { DbEntity.YearFrom = SetProperty(DbEntity.YearFrom, value); }
        }

        public int YearTo
        {
            get { return DbEntity.YearTo; }
            set { DbEntity.YearTo = SetProperty(DbEntity.YearTo, value); }
        }

        public event EventHandler DeleteRequested = delegate { };
        public RelayCommand DeleteCategoryCmd { get; private set; }

    }
}
