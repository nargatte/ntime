﻿using System.Collections.Generic;
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

        public virtual ICollection<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }
    }
}