using BaseCore.DataBase;

namespace BaseCore.PlayerFilter
{
    public class PlayerFilterOptions
    {

        public PlayerSort PlayerSort { get; set; }
        public bool DescendingSort { get; set; }
        public string Query { get; set; }
        public bool? Men { get; set; }
        public bool? WithoutStartTime { get; set; }
        public bool? Invalid { get; set; }
        public bool? CompleatedCompetition { get; set; }
        public bool? HasVoid { get; set; }
        public Distance Distance { get; set; }
        public AgeCategory AgeCategory { get; set; }
        public ExtraPlayerInfo ExtraPlayerInfo { get; set; }
    }
}