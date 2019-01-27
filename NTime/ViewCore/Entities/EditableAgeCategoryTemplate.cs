using BaseCore.DataBase;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableAgeCategoryTemplate : EditableItemBase<AgeCategoryCollection>
    {
        public EditableAgeCategoryTemplate()
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
        }


        public int Id
        {
            get { return DbEntity.Id; }
            set
            {
                DbEntity.Id = SetProperty(DbEntity.Id, value);
                OnUpdateRequested();
            }
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
