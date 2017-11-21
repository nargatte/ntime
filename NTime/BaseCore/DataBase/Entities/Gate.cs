using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Gate : IEntityId, ICompetitionId
    {
        public Gate()
        {
        }

        protected bool Equals(Gate other)
        {
            return Id == other.Id && string.Equals(Name, other.Name) && Number == other.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Gate) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Number;
                return hashCode;
            }
        }

        public Gate(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int Number { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<GatesOrderItem> GatesOrderItems { get; set; }

        public virtual ICollection<TimeReadsLogInfo> TimeReadsLogInfos { get; set; }

        public virtual  ICollection<TimeRead> TimeReads { get; set; }
    }
}