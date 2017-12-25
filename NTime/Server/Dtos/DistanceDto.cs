using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    [DataContract]
    public class DistanceDto : IDtoBase<Distance>
    {

        public DistanceDto()
        {

        }

        public DistanceDto(Distance distance)
        {
            Id = distance.Id;
            Name = distance.Name;
            Length = distance.Length;
            DistanceTypeEnum = distance.DistanceTypeEnum;
            LapsCount = distance.LapsCount;
            TimeLimit = distance.TimeLimit;
        }

        public Distance CopyDataFromDto(Distance distance)
        {
            distance.Id = Id;
            distance.Name = Name;
            distance.Length = Length;
            distance.DistanceTypeEnum = DistanceTypeEnum;
            distance.LapsCount = LapsCount;
            distance.TimeLimit = TimeLimit;
            return distance;
        }

        [DataMember]
        public int Id { get; set; }

        [StringLength(255), Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Length { get; set; }

        [DataMember]
        public int DistanceTypeId { get; set; }

        public DistanceTypeEnum DistanceTypeEnum
        {
            get => (DistanceTypeEnum)DistanceTypeId;
            set => DistanceTypeId = (int)value;
        }

        [DataMember]
        public int LapsCount { get; set; }

        [DataMember]
        public decimal TimeLimit { get; set; }
    }


}