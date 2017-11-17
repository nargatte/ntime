using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class LogsSource : IEntityId
    {
        public int Id { get; set; }

        public int FileNumber { get; set; }

        [StringLength(255)]
        public string Path { get; set; }

        public int GateId { get; set; }
        public virtual Gate Gate { get; set; }
    }
}