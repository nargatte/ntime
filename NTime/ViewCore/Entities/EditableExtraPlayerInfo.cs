using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace ViewCore.Entities
{
    public class EditableExtraPlayerInfo : EditableBaseClass<ExtraPlayerInfo>
    {
        public EditableExtraPlayerInfo(IEditableCompetition currentComptetition): base(currentComptetition) { }
        public string Name
        {
            get { return DbEntity.Name; }
            set { DbEntity.Name = SetProperty(DbEntity.Name, value); }
        }

        public string ShortName
        {
            get { return DbEntity.ShortName; }
            set { DbEntity.ShortName = SetProperty(DbEntity.ShortName, value); }
        }
    }
}
