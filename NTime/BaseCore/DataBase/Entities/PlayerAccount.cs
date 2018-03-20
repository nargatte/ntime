using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class PlayerAccount : IEntityId, IAccountId, IFullName
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string FirstName { get; set; }

        [StringLength(255)]
        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? IsMale { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [EmailAddress]
        public string EMail { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        public string AccountId { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}