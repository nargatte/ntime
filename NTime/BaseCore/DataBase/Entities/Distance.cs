using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Distance : IEntityId
    {
        public Distance()
        {
        }

        public Distance(string name, decimal length, DateTime startTime, int competitionId)
        {
            Name = name;
            Length = length;
            StartTime = startTime;
            CompetitionId = competitionId;
        }

        public int Id { get; set; }

        [StringLength(255), Required]
        public string Name { get; set; }

        public decimal Length { get; set; }

        public DateTime StartTime { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<ReaderOrder> ReaderOrders { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}