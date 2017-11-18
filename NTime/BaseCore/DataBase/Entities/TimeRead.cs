using System.ComponentModel.DataAnnotations.Schema;
using BaseCore.TimesProcess;

namespace BaseCore.DataBase
{
    public class TimeRead : IEntityId
    {
        public TimeRead()
        {
        }

        public TimeRead(decimal time)
        {
            Time = time;
        }

        public int Id { get; set; }

        public decimal Time { get; set; }

        public int? GateId { get; set; }
        public virtual Gate Gate { get; set; }

        public int TimeReadTypeId { get; set; }
        public virtual TimeReadType TimeReadType { get; set; }

        [NotMapped]
        public TimeReadTypeEnum TimeReadTypeEnum
        {
            get => (TimeReadTypeEnum) TimeReadTypeId;
            set => TimeReadTypeId = (int) value;
        }

        public int PlayerId { get; set; }
        public virtual Player Player { get; set; }

        public override string ToString()
        {
            return $"{nameof(Time)}: {Time.ToDateTime()}, {nameof(Gate.Number)}: {Gate.Number}, {nameof(TimeReadTypeEnum)}: {TimeReadTypeEnum}";
        }

        protected bool Equals(TimeRead other)
        {
            return Time == other.Time && Gate.Number == other.Gate.Number;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TimeRead) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Time.GetHashCode() * 397);
            }
        }
    }
}