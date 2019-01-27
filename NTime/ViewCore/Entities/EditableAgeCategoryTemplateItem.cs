using BaseCore.DataBase;
using MvvmHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableAgeCategoryTemplateItem : EditableItemBase<AgeCategoryTemplateItem>
    {
        public EditableAgeCategoryTemplateItem()
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
            Male = true;
        }

        public EditableAgeCategoryTemplateItem(EditableAgeCategoryTemplateItem editableAgeCategory)
        {
            Name = editableAgeCategory.Name;
            YearFrom = editableAgeCategory.YearFrom;
            YearTo = editableAgeCategory.YearTo;
            Male = editableAgeCategory.Male;
            AgeCategoryCollectionId = editableAgeCategory.AgeCategoryCollectionId;
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

        public bool Male
        {
            get { return DbEntity.Male; }
            set
            {
                DbEntity.Male = SetProperty(DbEntity.Male, value);
                OnUpdateRequested();
            }
        }

        public int AgeCategoryCollectionId
        {
            get { return DbEntity.AgeCategoryCollectionId; }
            set
            {
                DbEntity.AgeCategoryCollectionId = SetProperty(DbEntity.AgeCategoryCollectionId, value);
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
