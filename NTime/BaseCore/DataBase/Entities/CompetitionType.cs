using System.Collections.Generic;

namespace BaseCore.DataBase
{
    public class CompetitionType : EnumTable<CompetitionTypeEnum>
    {
        public CompetitionType()
        {
        }

        public CompetitionType(CompetitionTypeEnum @enum) : base(@enum)
        {
        }

        public static implicit operator CompetitionTypeEnum(CompetitionType competitionType) => (CompetitionTypeEnum)competitionType.Id;

        public static implicit operator CompetitionType(CompetitionTypeEnum @enum) => new CompetitionType(@enum);

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}