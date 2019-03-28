using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class ExtraColumnValue : IEntityId
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
        public int ColumnId { get; set; }
        public ExtraColumn Column { get; set; }
        public int LookupId { get; set; }
        public ExtraColumnValue Lookup { get; set; }
        public string CustomValue { get; set; }
    }
}
