using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BaseCore.DataBase;

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
            FirstName = organizerAccount.FirstName;
            LastName = organizerAccount.LastName;
            PhoneNumber = organizerAccount.PhoneNumber;
            EMail = organizerAccount.EMail;
        }

        public OrganizerAccount CopyDataFromDto(OrganizerAccount organizerAccount)
        {
            organizerAccount.Id = Id;
            organizerAccount.FirstName = FirstName;
            organizerAccount.LastName = LastName;
            organizerAccount.PhoneNumber = PhoneNumber;
            organizerAccount.EMail = EMail;
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
        public string EMail { get; set; }
    }
}