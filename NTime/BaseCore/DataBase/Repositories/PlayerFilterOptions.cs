namespace BaseCore.DataBase
{
    public class PlayerFilterOptions
    {
        public PlayerFilterOptions(PlayerSort playerSort = PlayerSort.ByLastName, bool descendingSort = false, string query = null, bool? men = null, bool? withoutStartTime = null, Distance distance = null, AgeCategory ageCategory = null, ExtraPlayerInfo extraPlayerInfo = null)
        {
            PlayerSort = playerSort;
            DescendingSort = descendingSort;
            Query = query;
            Men = men;
            WithoutStartTime = withoutStartTime;
            Distance = distance;
            AgeCategory = ageCategory;
            ExtraPlayerInfo = extraPlayerInfo;
        }

        public PlayerSort PlayerSort { get; }
        public bool DescendingSort { get; }
        public string Query { get; }
        public bool? Men { get; }
        public bool? WithoutStartTime { get; }
        public Distance Distance { get; }
        public AgeCategory AgeCategory { get; }
        public ExtraPlayerInfo ExtraPlayerInfo { get; }
    }
}