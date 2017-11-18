using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Gate : IEntityId, ICompetitionId
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int Number { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public virtual ICollection<GateOrderItem> GatesOrderItems { get; set; }

        public virtual ICollection<LogsSource> LogsSources { get; set; }
    }
}