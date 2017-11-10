using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace BaseCore.DataBase
{
    public class TimeRead : IEntityId
    {
        public TimeRead()
        {
        }

        public TimeRead(decimal time, int reader)
        {
            Time = time;
            Reader = reader;
        }

        public int Id { get; set; }

        public decimal Time { get; set; }

        public int Reader { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSkipped { get; set; }

        [NotMapped]
        public int StartNumber { get; set; }

        [Required]
        public int? PlayerId { get; set; }
        public virtual Player Player { get; set; }
    }
}