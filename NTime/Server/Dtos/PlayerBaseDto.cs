using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Server.Dtos
{
    public class PlayerBaseDto : IDtoBase<Player>
    {
        public PlayerBaseDto(Player player)
        {
            Id = player.Id;
            FirstName = player.FirstName;
            LastName = player.LastName;
            IsMale = player.IsMale;
            Team = player.Team;
            City = player.City;
        }


        public virtual Player CopyDataFromDto(Player player)
        {
            player.Id = Id;
            player.FirstName = FirstName;
            player.LastName = LastName;
            player.IsMale = IsMale;
            player.Team = Team;
            player.City = City;
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
        public string City { get; set; }
    }
}