using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCore.DataBase
{
    [Table("ExtraPlayerInfo")]
    public class ExtraPlayerInfo : IEntityId, ICompetitionId
    {
        public ExtraPlayerInfo()
        {
        }

        public ExtraPlayerInfo(string name, string shortName)
        {
            Name = name;
            ShortName = shortName;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        [StringLength(255), Required]
        public string ShortName { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; } 

        public virtual ICollection<Player> Players { get; set; }
    }
}