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

        public Distance(string name, decimal lenght, DateTime startTime, int competitionId)
        {
            Name = name;
            Lenght = lenght;
            StartTime = startTime;
            CompetitionId = competitionId;
        }

        public int Id { get; set; }

        [StringLength(256), Required]
        public string Name { get; set; }

        public decimal Lenght { get; set; }

        public DateTime StartTime { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<ReaderOrder> ReaderOrders { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}