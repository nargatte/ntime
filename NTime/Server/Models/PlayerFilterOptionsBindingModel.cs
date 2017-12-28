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

        public PlayerFilterOptions CreatePlayerFilterOptions()
        {
            PlayerFilterOptions filterOptions = new PlayerFilterOptions();
            filterOptions.PlayerSort = (PlayerSort) PlayerSort;
            filterOptions.DescendingSort = DescendingSort;
            filterOptions.Query = Query;
            filterOptions.Men = Men;
            filterOptions.WithoutStartTime = WithoutStartTime;
            filterOptions.Invalid = Invalid;
            filterOptions.CompleatedCompetition = CompleatedCompetition;
            filterOptions.HasVoid = HasVoid;
            if(Distance != null)
                filterOptions.Distance = new Distance(){Id = Distance.Value};
            if(AgeCategory != null)
                filterOptions.AgeCategory = new AgeCategory() {Id = AgeCategory.Value};
            if(ExtraPlayerInfo != null)
                filterOptions.ExtraPlayerInfo = new ExtraPlayerInfo() {Id = ExtraPlayerInfo.Value};
            return filterOptions;
        }
    }
}