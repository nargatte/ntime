﻿using BaseCore.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class OrganizerAccount : IEntityId, IAccountId
    {
        public int Id { get; set; }

        public string AccountId { get; set; }
        public virtual IApplicationUser Account { get; set; }

        public virtual ICollection<Competition> Competitions { get; set; }
    }
}