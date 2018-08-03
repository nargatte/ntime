using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class AgeCategoryDto : IDtoBase<AgeCategory>
    {
        public AgeCategoryDto()
        {

        }

        public AgeCategoryDto(AgeCategory ageCategory)
        {
            Id = ageCategory.Id;
            Name = ageCategory.Name;
            YearFrom = ageCategory.YearFrom;
            YearTo = ageCategory.YearTo;
            Male = ageCategory.Male;
        }

        public AgeCategory CopyDataFromDto(AgeCategory ageCategory)
        {
            ageCategory.Id = Id;
            ageCategory.Name = Name;
            ageCategory.YearFrom = YearFrom;
            ageCategory.YearTo = YearTo;
            ageCategory.Male = Male;
            return ageCategory;
        }


        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public int YearFrom { get; set; }

        public int YearTo { get; set; }

        public bool Male { get; set; }
    }
}