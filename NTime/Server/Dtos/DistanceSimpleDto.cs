using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class DistanceSimpleDto : IDtoBase<Distance>
    {
        public DistanceSimpleDto()
        {

        }

        public DistanceSimpleDto(Distance distance)
        {
            Id = distance.Id;
            Name = distance.Name;
            Length = distance.Length;
        }

        public Distance CopyDataFromDto(Distance distance)
        {
            distance.Id = Id;
            distance.Name = Name;
            distance.Length = Length;
            return distance;
        }



        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public decimal Length { get; set; }
    }
}