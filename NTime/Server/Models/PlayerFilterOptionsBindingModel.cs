using BaseCore.DataBase;
using BaseCore.PlayerFilter;

namespace Server.Models
{
    public class PlayerFilterOptionsBindingModel
    {
        public int PlayerSort { get; set; }
        public bool DescendingSort { get; set; }
        public string Query { get; set; }
        public bool? Men { get; set; }
        public bool? WithoutStartTime { get; set; }
        public bool? Invalid { get; set; }
        public bool? CompleatedCompetition { get; set; }
        public bool? HasVoid { get; set; }
        public int? Distance { get; set; }
        public int? AgeCategory { get; set; }
        public int? ExtraPlayerInfo { get; set; }

        public PlayerFilterOptionsBindingModel()
        {

        }

        public PlayerFilterOptionsBindingModel(PlayerFilterOptions filterOptions)
        {
            PlayerSort = (int)filterOptions.PlayerSort;
            DescendingSort = filterOptions.DescendingSort;
            Query = filterOptions.Query;
            Men = filterOptions.Men;
            WithoutStartTime = filterOptions.WithoutStartTime;
            Invalid = filterOptions.Invalid;
            CompleatedCompetition = filterOptions.CompleatedCompetition;
            HasVoid = filterOptions.HasVoid;
            if (filterOptions.Distance != null)
                Distance = filterOptions.Distance.Id;
            if (filterOptions.AgeCategory != null)
                AgeCategory = filterOptions.AgeCategory.Id;
            if (filterOptions.ExtraPlayerInfo != null)
                ExtraPlayerInfo = filterOptions.ExtraPlayerInfo.Id;
        }

        public PlayerFilterOptions CreatePlayerFilterOptions()
        {
            PlayerFilterOptions filterOptions = new PlayerFilterOptions();
            filterOptions.PlayerSort = (PlayerSort)PlayerSort;
            filterOptions.DescendingSort = DescendingSort;
            filterOptions.Query = Query;
            filterOptions.Men = Men;
            filterOptions.WithoutStartTime = WithoutStartTime;
            filterOptions.Invalid = Invalid;
            filterOptions.CompleatedCompetition = CompleatedCompetition;
            filterOptions.HasVoid = HasVoid;
            if (Distance != null)
                filterOptions.Distance = new Distance() { Id = Distance.Value };
            if (AgeCategory != null)
                filterOptions.AgeCategory = new AgeCategory() { Id = AgeCategory.Value };
            if (ExtraPlayerInfo != null)
                filterOptions.ExtraPlayerInfo = new ExtraPlayerInfo() { Id = ExtraPlayerInfo.Value };
            return filterOptions;
        }
    }
}