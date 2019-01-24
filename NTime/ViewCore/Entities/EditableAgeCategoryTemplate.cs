using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableAgeCategoryTemplate : EditableBaseClass<AgeCategoryTemplate>
    {
        public EditableAgeCategoryTemplate(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
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
