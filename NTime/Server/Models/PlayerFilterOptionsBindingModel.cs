using BaseCore.DataBase;
using BaseCore.PlayerFilter;

namespace Server.Models
{
    public class PlayerFilterOptionsBindingModel
    {
        public int PlayerSort { get; set; }
        public bool DescendingSort { get; set; }
        public int ExtraDataSortIndex { get; set; }
        public string Query { get; set; }
        public bool? IsMale { get; set; }
        public bool? WithoutStartTime { get; set; }
        public bool? Invalid { get; set; }
        public bool? CompletedCompetition { get; set; }
        public bool? HasVoid { get; set; }
        public bool? IsPaidUp { get; set; }
        public int? Distance { get; set; }
        public int? AgeCategory { get; set; }
        public int? Subcategory { get; set; }

        public PlayerFilterOptionsBindingModel()
        {

        }

        public PlayerFilterOptionsBindingModel(PlayerFilterOptions filterOptions)
        {
            PlayerSort = (int)filterOptions.PlayerSort;
            DescendingSort = filterOptions.DescendingSort;
            ExtraDataSortIndex = filterOptions.ExtraDataSortIndex;
            Query = filterOptions.Query;
            IsMale = filterOptions.IsMale;
            WithoutStartTime = filterOptions.WithoutStartTime;
            Invalid = filterOptions.Invalid;
            CompletedCompetition = filterOptions.CompletedCompetition;
            HasVoid = filterOptions.HasVoid;
            IsPaidUp = filterOptions.IsPaidUp;
            if (filterOptions.Distance != null)
                Distance = filterOptions.Distance.Id;
            if (filterOptions.AgeCategory != null)
                AgeCategory = filterOptions.AgeCategory.Id;
            if (filterOptions.Subcategory != null)
                Subcategory = filterOptions.Subcategory.Id;
        }

        public PlayerFilterOptions CreatePlayerFilterOptions()
        {
            PlayerFilterOptions filterOptions = new PlayerFilterOptions();
            filterOptions.PlayerSort = (PlayerSort)PlayerSort;
            filterOptions.DescendingSort = DescendingSort;
            filterOptions.ExtraDataSortIndex = ExtraDataSortIndex;
            filterOptions.Query = Query;
            filterOptions.IsMale = IsMale;
            filterOptions.WithoutStartTime = WithoutStartTime;
            filterOptions.Invalid = Invalid;
            filterOptions.CompletedCompetition = CompletedCompetition;
            filterOptions.HasVoid = HasVoid;
            filterOptions.IsPaidUp = IsPaidUp;
            if (Distance != null)
                filterOptions.Distance = new Distance() { Id = Distance.Value };
            if (AgeCategory != null)
                filterOptions.AgeCategory = new AgeCategory() { Id = AgeCategory.Value };
            if (Subcategory != null)
                filterOptions.Subcategory = new Subcategory() { Id = Subcategory.Value };
            return filterOptions;
        }
    }
}