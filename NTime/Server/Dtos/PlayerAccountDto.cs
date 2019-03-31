using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;
using Server.Models;

namespace Server.Dtos
{
    public class PlayerAccountDto : PlayerBaseDto
    {
        public PlayerAccountDto(PlayerAccount account, ApplicationUser applicationUser) 
            : base(new Player(account, applicationUser.FirstName, applicationUser.LastName))  
        {
            Email = applicationUser.Email;
            PhoneNumber = applicationUser.PhoneNumber;
        }

        public (PlayerAccount, ApplicationUser) CopyDataFromDto(PlayerAccount playerAccount, ApplicationUser applicationUser)
        {
            base.CopyDataFromDto(new Player(playerAccount, applicationUser.FirstName, applicationUser.LastName));
            applicationUser.PhoneNumber = PhoneNumber;
            applicationUser.Email = Email;
            return (playerAccount, applicationUser);
        }

        public DateTime? BirthDate { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}