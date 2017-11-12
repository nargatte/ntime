using System.Collections.Generic;

namespace BaseCore.DataBase
{
    public class DistanceType : EnumTable<DistanceTypeEnum>
    {
        public DistanceType()
        {
        }

        public DistanceType(DistanceTypeEnum @enum) : base(@enum)
        {
        }

        public static implicit operator DistanceTypeEnum(DistanceType distanceType) => (DistanceTypeEnum)distanceType.Id;

        public static implicit operator DistanceType(DistanceTypeEnum @enum) => new DistanceType(@enum);

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}