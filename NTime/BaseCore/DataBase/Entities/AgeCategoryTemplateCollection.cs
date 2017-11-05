using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class AgeCategoryTemplateCollection
    {
        public AgeCategoryTemplateCollection()
        {
        }

        public AgeCategoryTemplateCollection(string name)
        {
            Name = name;
        }

        public int Id { get; set; }

        [StringLength(256), Required]
        public string Name { get; set; }

        public virtual ObservableCollection<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }
    }
}