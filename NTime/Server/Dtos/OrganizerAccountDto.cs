using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;
using Server.Models;

namespace Server.Dtos
{
    public class OrganizerAccountDto : IDtoBase<OrganizerAccount>
    {
        public OrganizerAccountDto()
        {

        }

        public OrganizerAccountDto(OrganizerAccount organizerAccount)
        {
            Id = organizerAccount.Id;
        }

        public OrganizerAccountDto(OrganizerAccount organizerAccount, ApplicationUser applicationUser)
        : this(organizerAccount)
        {
            FirstName = applicationUser.FirstName;
            LastName = applicationUser.LastName;
            PhoneNumber = applicationUser.PhoneNumber;
            Email = applicationUser.Email;
        }

        public OrganizerAccount CopyDataFromDto(OrganizerAccount organizerAccount)
        {
            organizerAccount.Id = Id;
            return organizerAccount;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public CompetitionDto[] CompetitionDtos { get; set; }
    }
}