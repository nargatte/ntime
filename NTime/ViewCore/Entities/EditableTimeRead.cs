using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableTimeRead : EditableBaseClass<TimeRead>
    {

        public decimal Time
        {
            get { return DbEntity.Time; }
            set { DbEntity.Time = SetProperty(DbEntity.Time, value); }
        }

        public int Reader
        {
            get { return DbEntity.Reader; }
            set { DbEntity.Reader = SetProperty(DbEntity.Reader, value); }
        }

    }
}
