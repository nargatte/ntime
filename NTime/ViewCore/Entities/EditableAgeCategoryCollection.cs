using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore.Entities
{
    public class EditableAgeCategoryCollection : EditableItemBase<AgeCategoryCollection>
    {
        public EditableAgeCategoryCollection()
        {
        }


        public int Id
        {
            get { return DbEntity.Id; }
            set { DbEntity.Id = SetProperty(DbEntity.Id, value); }
        }


        public string Name
        {
            get { return DbEntity.Name; }
            set { DbEntity.Name = SetProperty(DbEntity.Name, value); }
        }
    }
}
