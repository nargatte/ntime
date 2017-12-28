using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerListViewDto : IDtoBase<Player>
    {
        public PlayerListViewDto()
        {

        }

        public PlayerListViewDto(Player player)
        {
            Id = player.Id;
            FirstName = player.FirstName;
            LastName = player.LastName;
            IsMale = player.IsMale;
            Team = player.Team;
            StartNumber = player.StartNumber;
            StartTime = player.StartTime;
            FullCategory = player.FullCategory;
        }

        public Player CopyDataFromDto(Player player)
        {
            player.Id = Id;
            player.FirstName = FirstName;
            player.LastName = LastName;
            player.IsMale = IsMale;
            player.Team = Team;
            player.StartNumber = StartNumber;
            player.StartTime = StartTime;
            player.FullCategory = FullCategory;
            return player;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        public bool IsMale { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        public int StartNumber { get; set; }

        public DateTime? StartTime { get; set; }

        [StringLength(255)]
        public string FullCategory { get; set; }
    }
}