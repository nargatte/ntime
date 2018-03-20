﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerCompetitionRegisterDto : IDtoBase<Player>
    {
        public PlayerCompetitionRegisterDto()
        {

        }

        public PlayerCompetitionRegisterDto(Player player)
        {
            Id = player.Id;
            FirstName = player.FirstName;
            LastName = player.LastName;
            BirthDate = player.BirthDate;
            IsMale = player.IsMale;
            Team = player.Team;
            City = player.City;
            Email = player.Email;
            PhoneNumber = player.PhoneNumber;
            ExtraPlayerInfoId = player.ExtraPlayerInfo.Id;
            DistanceId = player.Distance.Id;
            CompetitionId = player.CompetitionId;
        }

        public Player CopyDataFromDto(Player player)
        {
            player.Id = Id;
            player.FirstName = FirstName;
            player.LastName = LastName;
            player.BirthDate = BirthDate;
            player.IsMale = IsMale;
            player.Team = Team;
            player.City = City;
            player.Email = Email;
            player.PhoneNumber = PhoneNumber;
            player.ExtraPlayerInfoId = ExtraPlayerInfoId;
            player.DistanceId = DistanceId;
            player.CompetitionId = CompetitionId;
            return player;
        }

        public async Task<Player> CopyDataFromDto(Player player, IContextProvider contextProvider, Competition competition)
        {
            ExtraPlayerInfoRepository extraPlayerInfoRepository = new ExtraPlayerInfoRepository(contextProvider, competition);
            DistanceRepository distanceRepository = new DistanceRepository(contextProvider, competition);

            CopyDataFromDto(player);
            player.ExtraPlayerInfo = await extraPlayerInfoRepository.GetById(ExtraPlayerInfoId);
            player.Distance = await distanceRepository.GetById(DistanceId);

            if (player.ExtraPlayerInfo == null || player.Distance == null)
                return null;
            return player;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool IsMale { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        public string City { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int ExtraPlayerInfoId { get; set; }

        public int DistanceId { get; set; }

        public int CompetitionId { get; set; }
    }
}