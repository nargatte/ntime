﻿using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Server.Dtos
{
    public class PlayerPublicDetailsDto : PlayerListViewDto, IDtoBase<Player>
    {
        public PlayerPublicDetailsDto()
        {

        }
        public PlayerPublicDetailsDto(Player player) : base(player)
        {
            RegistrationDate = player.RegistrationDate;
            LapsCount = player.LapsCount;
            Time = player.Time;
            DistancePlaceNumber = player.DistancePlaceNumber;
            CategoryPlaceNumber = player.CategoryPlaceNumber;
            CompetitionCompleted = player.CompetitionCompleted;
            SubcategoryId = player.Subcategory == null ? -1 : player.Subcategory.Id;
            DistanceId = player.Distance == null ? -1 : player.Distance.Id;
            AgeCategoryId = player.AgeCategory == null ? -1 : player.AgeCategory.Id;
            PlayerAccountId = player.PlayerAccountId;
            ExtraColumnValues = player.ExtraColumnValues
                .Where(columnValue => columnValue.Column.IsDisplayedInPublicDetails)
                .Select(columnValue => new ExtraColumnValueDto(columnValue)).ToArray();
        }

        public override Player CopyDataFromDto(Player player)
        {
            base.CopyDataFromDto(player);
            player.ExtraColumnValues = ExtraColumnValues.Select(column => column.CopyDataFromDto(new ExtraColumnValue())).ToArray();
            player.LapsCount = LapsCount;
            player.Time = Time;
            player.DistancePlaceNumber = DistancePlaceNumber;
            player.CategoryPlaceNumber = CategoryPlaceNumber;
            player.CompetitionCompleted = CompetitionCompleted;
            player.SubcategoryId = SubcategoryId;
            player.DistanceId = DistanceId;
            player.AgeCategoryId = AgeCategoryId;
            player.PlayerAccountId = PlayerAccountId;
            return player;
        }


        public async Task<Player> CopyDataFromDto(Player player, IContextProvider contextProvider, Competition competition)
        {
            SubcategoryRepository subcategoryRepository = new SubcategoryRepository(contextProvider, competition);
            DistanceRepository distanceRepository = new DistanceRepository(contextProvider, competition);
            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(contextProvider, competition);

            CopyDataFromDto(player);
            player.Subcategory = await subcategoryRepository.GetById(SubcategoryId);
            player.Distance = await distanceRepository.GetById(DistanceId);
            player.AgeCategory = await ageCategoryRepository.GetById(AgeCategoryId);

            if (player.Subcategory == null || player.Distance == null || player.AgeCategory == null)
                return null;

            if (player.AgeCategory.Male != player.IsMale) return null;
            if (!(await ageCategoryRepository.IsAgeCategoryAndDistanceMatch(player.AgeCategory, player.Distance)))
                return null;

            return player;
        }

        public DateTime RegistrationDate { get; set; }
        public int LapsCount { get; set; }
        public decimal Time { get; set; }
        public int DistancePlaceNumber { get; set; }
        public int CategoryPlaceNumber { get; set; }
        public bool CompetitionCompleted { get; set; }
        public int SubcategoryId { get; set; }
        public int DistanceId { get; set; }
        public int AgeCategoryId { get; set; }
        public int? PlayerAccountId { get; set; }
    }
}