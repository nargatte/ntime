﻿using BaseCore.DataBase;
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
        public int? Subcategory { get; set; }

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
            CompleatedCompetition = filterOptions.CompletedCompetition;
            HasVoid = filterOptions.HasVoid;
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
            filterOptions.Query = Query;
            filterOptions.Men = Men;
            filterOptions.WithoutStartTime = WithoutStartTime;
            filterOptions.Invalid = Invalid;
            filterOptions.CompletedCompetition = CompleatedCompetition;
            filterOptions.HasVoid = HasVoid;
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