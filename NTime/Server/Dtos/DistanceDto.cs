using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class DistanceDto
    {

        public DistanceDto()
        {

        }

        public DistanceDto(Distance distance)
        {
            Id = distance.Id;
            Name = distance.Name;
            Length = distance.Length;
            StartTime = distance.StartTime;
            DistanceTypeEnum = distance.DistanceTypeEnum;
            LapsCount = distance.LapsCount;
            TimeLimit = distance.TimeLimit;
        }

        public Distance CopyDataFromDto(Distance distance)
        {
            distance.Id = Id;
            distance.Name = Name;
            distance.Length = Length;
            distance.StartTime = StartTime;
            distance.DistanceTypeEnum = DistanceTypeEnum;
            distance.LapsCount = LapsCount;
            distance.TimeLimit = TimeLimit;
            return distance;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public decimal Length { get; set; }

        public DateTime StartTime { get; set; }

        public int DistanceTypeId { get; set; }
        public virtual DistanceType DistanceType { get; set; }

        public DistanceTypeEnum DistanceTypeEnum
        {
            get => (DistanceTypeEnum)DistanceTypeId;
            set => DistanceTypeId = (int)value;
        }

        public int LapsCount { get; set; }

        public decimal TimeLimit { get; set; }
    }


}