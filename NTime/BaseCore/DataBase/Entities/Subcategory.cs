using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCore.DataBase
{
    [Table("Subcategory")]
    public class Subcategory : IEntityId, ICompetitionId
    {
        public Subcategory()
        {
        }

        protected bool Equals(Subcategory other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Subcategory) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public Subcategory(string name, string shortName, bool male)
        {
            Name = name;
            ShortName = shortName;
            Male = male;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        [StringLength(255), Required]
        public string ShortName { get; set; }

        public bool Male { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; } 

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<SubcategoryDistance> SubcategoryDistances { get; set; }
    }
}