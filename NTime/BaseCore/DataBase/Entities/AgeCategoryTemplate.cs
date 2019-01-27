using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplate : IEntityId
    {
        public AgeCategoryTemplate()
        {
        }

        public AgeCategoryTemplate(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public virtual ICollection<AgeCategoryTemplateItem> AgeCategoryTemplateItems { get; set; }
    }
}