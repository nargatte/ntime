using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class OrganizerAccount : IEntityId, IAccountId
    {
        public int Id { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string EMail { get; set; }

        public int AccountId { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}