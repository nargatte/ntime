using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class ExtraPlayerInfo
    {
        public ExtraPlayerInfo()
        {
        }

        public int Id { get; set; }

        [StringLength(256), Required]
        public string Name { get; set; }

        [StringLength(256), Required]
        public string ShortName { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; } 

        public virtual ICollection<Player> Players { get; set; }
    }
}