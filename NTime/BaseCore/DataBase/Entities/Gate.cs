using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Gate : IEntityId, ICompetitionId
    {
        public Gate()
        {
        }

        public Gate(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int Number { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<GatesOrderItem> GatesOrderItems { get; set; }

        public virtual ICollection<TimeReadsLogInfo> TimeReadsLogInfos { get; set; }

        public virtual  ICollection<TimeRead> TimeReads { get; set; }
    }
}