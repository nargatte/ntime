using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableAgeCategoryTemplate : EditableItemBase<AgeCategoryTemplate>
    {
        public EditableAgeCategoryTemplate()
        {
        }

        public EditableAgeCategoryTemplate(EditableAgeCategoryTemplate editableAgeCategory)
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
            }
        }

        public int YearFrom
        {
            get { return DbEntity.YearFrom; }
            set
            {
                DbEntity.YearFrom = SetProperty(DbEntity.YearFrom, value);
            }
        }

        public int YearTo
        {
            get { return DbEntity.YearTo; }
            set
            {
                DbEntity.YearTo = SetProperty(DbEntity.YearTo, value);
            }
        }

        public bool Male
        {
            get { return DbEntity.Male; }
            set
            {
                DbEntity.Male = SetProperty(DbEntity.Male, value);
            }
        }

        public int AgeCategoryCollectionId
        {
            get { return DbEntity.AgeCategoryCollectionId; }
            set { DbEntity.AgeCategoryCollectionId = SetProperty(DbEntity.AgeCategoryCollectionId, value); }
        }
    }
}
