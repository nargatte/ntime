using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;

namespace ViewCore.Entities
{
    public class EditableSubcategory : EditableCompetitionItemBase<Subcategory>
    {
        public EditableSubcategory(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            DeleteSubcategoryCmd = new RelayCommand(OnDeleteSubcategory);
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

        public string ShortName
        {
            get { return DbEntity.ShortName; }
            set
            {
                DbEntity.ShortName = SetProperty(DbEntity.ShortName, value);
                OnUpdateRequested();
            }
        }

        private void OnDeleteSubcategory()
        {
            DeleteRequested(this, EventArgs.Empty);
        }

        public RelayCommand DeleteSubcategoryCmd { get; private set; }
        public event EventHandler DeleteRequested = delegate { };
        public event EventHandler UpdateRequested = delegate { };

        protected void OnUpdateRequested()
        {
            UpdateRequested?.Invoke(this, EventArgs.Empty);
        }

    }
}
