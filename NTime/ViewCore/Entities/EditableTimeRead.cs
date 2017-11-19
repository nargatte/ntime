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
        public EditableTimeRead(IEditableCompetition currentCompetition) : base(currentCompetition) { }
        public decimal Time
        {
            get { return DbEntity.Time; }
            set { DbEntity.Time = SetProperty(DbEntity.Time, value); }
        }

        public int Reader
        {
            get { return DbEntity.Gate.Number; }
            set { DbEntity.Gate.Number = SetProperty(DbEntity.Gate.Number, value); }
        }

    }
}
