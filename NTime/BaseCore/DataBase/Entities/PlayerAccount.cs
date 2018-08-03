using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class PlayerAccount : IEntityId, IAccountId
    {
        public int Id { get; set; }

        public DateTime? BirthDate { get; set; }

        public bool? IsMale { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        public string AccountId { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}