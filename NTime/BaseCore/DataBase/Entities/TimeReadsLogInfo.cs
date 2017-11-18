using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class TimeReadsLogInfo : IEntityId
    {
        public int Id { get; set; }

        public int LogNumber { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        public int GateId { get; set; }
        public virtual Gate Gate { get; set; }
    }
}