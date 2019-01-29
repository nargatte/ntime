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
        public EditableAgeCategoryTemplateItem(bool isEditable)
        {
            DeleteCategoryCmd = new RelayCommand(OnDeleteCategory);
            Male = true;
            IsEditable = isEditable;
        }

        public EditableAgeCategoryTemplateItem(EditableAgeCategoryTemplateItem editableAgeCategory) : this(editableAgeCategory.IsEditable)
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

        public int AgeCategoryCollectionId
        {
            get { return DbEntity.AgeCategoryTemplateId; }
            set
            {
                DbEntity.AgeCategoryTemplateId = SetProperty(DbEntity.AgeCategoryTemplateId, value);
                OnUpdateRequested();
            }
        }


        private bool _isEditable;
        public bool IsEditable
        {
            get { return _isEditable; }
            set { SetProperty(ref _isEditable, value); }
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
