using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class AgeCategoryCollection : IEntityId
    {
        public AgeCategoryCollection()
        {
        }

        public AgeCategoryCollection(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public virtual ICollection<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }
    }
}