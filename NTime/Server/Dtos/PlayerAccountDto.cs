using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;
using Server.Models;

namespace Server.Dtos
{
    public class PlayerAccountDto : IDtoBase<PlayerAccount>
    {
        public PlayerAccountDto()
        {

        }

        /// <summary>
        /// Use PlayerAccountDto(PlayerAccount account, ApplicationUser applicationUser) insstead
        /// </summary>
        /// <param name="playerAccount"></param>
        public PlayerAccountDto(PlayerAccount playerAccount)
        {
            Id = playerAccount.Id;
            BirthDate = playerAccount.BirthDate;
            IsMale = playerAccount.IsMale;
            Team = playerAccount.Team;
            City = playerAccount.City;
        }

        public PlayerAccountDto(PlayerAccount account, ApplicationUser applicationUser)
            : this(account)
        {
            FirstName = applicationUser.FirstName;
            LastName = applicationUser.LastName;
            EMail = applicationUser.Email;
            PhoneNumber = applicationUser.PhoneNumber;
        }

        public PlayerAccount CopyDataFromDto(PlayerAccount playerAccount)
        {
            playerAccount.Id = Id;
            playerAccount.BirthDate = BirthDate;
            playerAccount.IsMale = IsMale;
            playerAccount.Team = Team;
            playerAccount.City = City;
            return playerAccount;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }

        public bool? IsMale { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string EMail { get; set; }

        public string City { get; set; }
    }
}