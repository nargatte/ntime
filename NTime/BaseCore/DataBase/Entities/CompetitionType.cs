using System.Collections.Generic;

namespace BaseCore.DataBase
{
    public class CompetitionType : EnumTable<DistanceTypeEnum>
    {
        public CompetitionType()
        {
        }

        public CompetitionType(DistanceTypeEnum @enum) : base(@enum)
        {
        }

        public static implicit operator DistanceTypeEnum(CompetitionType competitionType) => (DistanceTypeEnum)competitionType.Id;

        public static implicit operator CompetitionType(DistanceTypeEnum @enum) => new CompetitionType(@enum);

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}