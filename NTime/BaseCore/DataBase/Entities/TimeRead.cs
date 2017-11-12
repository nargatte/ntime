using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCore.DataBase
{
    public class TimeRead : IEntityId
    {
        public TimeRead()
        {
        }

        public TimeRead(decimal time, int reader)
        {
            Time = time;
            Reader = reader;
        }

        public int Id { get; set; }

        public decimal Time { get; set; }

        public int Reader { get; set; }

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

        protected bool Equals(TimeRead other)
        {
            return Time == other.Time && Reader == other.Reader;
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
                return (Time.GetHashCode() * 397) ^ Reader;
            }
        }
    }
}