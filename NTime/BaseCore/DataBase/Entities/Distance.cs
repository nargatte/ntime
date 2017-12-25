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

        protected bool Equals(Distance other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && Length == other.Length && DistanceTypeId == other.DistanceTypeId && LapsCount == other.LapsCount && TimeLimit == other.TimeLimit && CompetitionId == other.CompetitionId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Distance) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Length.GetHashCode();
                hashCode = (hashCode * 397) ^ DistanceTypeId;
                hashCode = (hashCode * 397) ^ LapsCount;
                hashCode = (hashCode * 397) ^ TimeLimit.GetHashCode();
                hashCode = (hashCode * 397) ^ CompetitionId;
                return hashCode;
            }
        }

        public Distance(string name, decimal length, DistanceTypeEnum distanceTypeEnum)
        {
            Name = name;
            Length = length;
            DistanceTypeEnum = distanceTypeEnum;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public decimal Length { get; set; }

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

        public virtual ICollection<GatesOrderItem> GatesOrders { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Gate> Gates { get; set; }
    }
}