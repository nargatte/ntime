using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class SubcategoryDto : IDtoBase<Subcategory>
    {
        public SubcategoryDto()
        {

        }

        public SubcategoryDto(Subcategory subcategory)
        {
            Id = subcategory.Id;
            Name = subcategory.Name;
            ShortName = subcategory.ShortName;
        }

        public Subcategory CopyDataFromDto(Subcategory subcategory)
        {
            subcategory.Id = Id;
            subcategory.Name = Name;
            subcategory.ShortName = ShortName;
            return subcategory;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        [StringLength(255), Required]
        public string ShortName { get; set; }

        public bool Male { get; set; }
    }
}