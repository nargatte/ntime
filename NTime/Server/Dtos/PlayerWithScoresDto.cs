using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerWithScoresDto : PlayerPublicDetailsDto, IDtoBase<Player>
    {

        public PlayerWithScoresDto(Player player) : base(player)
        {
            BirthDate = player.BirthDate;
            PhoneNumber = player.PhoneNumber;
            Email = player.Email;
            IsStartTimeFromReader = player.IsStartTimeFromReader;
            IsCategoryFixed = player.IsCategoryFixed;
            ExtraColumnValues = player.ExtraColumnValues
                .Select(columnValue => new ExtraColumnValueDto(columnValue)).ToArray();
        }

        public override Player CopyDataFromDto(Player player)
        {
            base.CopyDataFromDto(player);
            player.BirthDate = BirthDate;
            player.PhoneNumber = PhoneNumber;
            player.Email = Email;
            player.IsStartTimeFromReader = IsStartTimeFromReader;
            player.IsCategoryFixed = IsCategoryFixed;
            return player;
        }


        [Required]
        public DateTime BirthDate { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool IsStartTimeFromReader { get; set; }
        public bool IsCategoryFixed { get; set; }
    }
}