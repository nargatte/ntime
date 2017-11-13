using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCore.DataBase
{
    public class Distance : IEntityId, ICompetitionId
    {
        public Distance()
        {
        }

        public Distance(string name, decimal length, DateTime startTime, DistanceTypeEnum distanceTypeEnum)
        {
            Name = name;
            Length = length;
            StartTime = startTime;
            DistanceTypeEnum = distanceTypeEnum;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public decimal Length { get; set; }

        public DateTime StartTime { get; set; }

        public int DistanceTypeId { get; set; }
        public virtual DistanceType DistanceType { get; set; }

        [NotMapped]
        public DistanceTypeEnum DistanceTypeEnum
        {
            get => (DistanceTypeEnum)DistanceTypeId;
            set => DistanceTypeId = (int)value;
        }

        public int LapsCount { get; set; }

        public decimal TimeLimit { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<ReaderOrder> ReaderOrders { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}