using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Distance : IEntityId, ICompetitionId
    {
        public Distance()
        {
        }

        public Distance(string name, decimal length, DateTime startTime)
        {
            Name = name;
            Length = length;
            StartTime = startTime;
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