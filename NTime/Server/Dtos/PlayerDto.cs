using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerDto : IDtoBase<Player>
    {
        public PlayerDto()
        {

        }

        public PlayerDto(Player player)
        {
            Id = player.Id;
            FirstName = player.FirstName;
            LastName = player.LastName;
            BirthDate = player.BirthDate;
            IsMale = player.IsMale;
            Team = player.Team;
            StartNumber = player.StartNumber;
            StartTime = player.StartTime;
            IsStartTimeFromReader = player.IsStartTimeFromReader;
            FullCategory = player.FullCategory;
            PhoneNumber = player.PhoneNumber;
        }

        public Player CopyDataFromDto(Player player)
        {
            player.Id = Id;
            player.FirstName = FirstName;
            player.LastName = LastName;
            player.BirthDate = BirthDate;
            player.IsMale = IsMale;
            player.Team = Team;
            player.StartNumber = StartNumber;
            player.StartTime = StartTime;
            player.IsStartTimeFromReader = IsStartTimeFromReader;
            player.FullCategory = FullCategory;
            player.PhoneNumber = PhoneNumber;
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

        [StringLength(255)]
        public string Team { get; set; }

        public int StartNumber { get; set; }

        public DateTime? StartTime { get; set; }

        public bool IsStartTimeFromReader { get; set; }

        [StringLength(255)]
        public string FullCategory { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}