using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerAccountDto
    {
        public PlayerAccountDto()
        {

        }

        public PlayerAccountDto(PlayerAccount playerAccount)
        {
            Id = playerAccount.Id;
            FirstName = playerAccount.FirstName;
            LastName = playerAccount.LastName;
            BirthDate = playerAccount.BirthDate;
            IsMale = playerAccount.IsMale;
            Team = playerAccount.Team;
            PhoneNumber = playerAccount.PhoneNumber;
            EMail = playerAccount.EMail;
        }

        public PlayerAccount CopyDataFromDto(PlayerAccount playerAccount)
        {
            playerAccount.Id = Id;
            playerAccount.FirstName = FirstName;
            playerAccount.LastName = LastName;
            playerAccount.BirthDate = BirthDate;
            playerAccount.IsMale = IsMale;
            playerAccount.Team = Team;
            playerAccount.PhoneNumber = PhoneNumber;
            playerAccount.EMail = EMail;
            return playerAccount;
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

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string EMail { get; set; }
    }
}