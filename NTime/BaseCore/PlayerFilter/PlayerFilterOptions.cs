using BaseCore.DataBase;

namespace BaseCore.PlayerFilter
{
    public class PlayerFilterOptions
    {
        public PlayerFilterOptions(PlayerSort playerSort, bool descendingSort, string query, bool? men, bool? withoutStartTime, bool? invalid, Distance distance, AgeCategory ageCategory, ExtraPlayerInfo extraPlayerInfo)
        {
            PlayerSort = playerSort;
            DescendingSort = descendingSort;
            Query = query;
            Men = men;
            WithoutStartTime = withoutStartTime;
            Invalid = invalid;
            Distance = distance;
            AgeCategory = ageCategory;
            ExtraPlayerInfo = extraPlayerInfo;
        }

        public PlayerSort PlayerSort { get; }
        public bool DescendingSort { get; }
        public string Query { get; }
        public bool? Men { get; }
        public bool? WithoutStartTime { get; }
        public bool? Invalid { get; }
        public Distance Distance { get; }
        public AgeCategory AgeCategory { get; }
        public ExtraPlayerInfo ExtraPlayerInfo { get; }
    }
}