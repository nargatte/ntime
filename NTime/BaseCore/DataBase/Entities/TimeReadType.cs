using System.Collections.Generic;

namespace BaseCore.DataBase
{
    public class TimeReadType : EnumTable<TimeReadTypeEnum>
    {
        public TimeReadType()
        {
        }

        public TimeReadType(TimeReadTypeEnum @enum) : base(@enum)
        {
        }

        public static implicit operator TimeReadTypeEnum(TimeReadType distanceType) => (TimeReadTypeEnum)distanceType.Id;

        public static implicit operator TimeReadType(TimeReadTypeEnum @enum) => new TimeReadType(@enum);

        public virtual ICollection<TimeRead> Competitions { get; set; }
    }
}