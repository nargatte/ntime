using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Runtime.Remoting.Messaging;

namespace BaseCore.DataBase
{
    public class Competition : IEntityId
    {
        public Competition()
        {
        }

        public Competition(string name, DateTime eventDate, string description, string link, string organiser, string city, CompetitionTypeEnum competitionTypeEnum)
        {
            Name = name;
            EventDate = eventDate;
            Description = description;
            Link = link;
            Organiser = organiser;
            City = city;
            CompetitionTypeId = (int)competitionTypeEnum;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public string Description { get; set; }

        [StringLength(2000)]
        public string Link { get; set; }

        [StringLength(255)]
        public string Organiser { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        public int CompetitionTypeId { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }

        [NotMapped]
        public CompetitionTypeEnum CompetitionTypeEnum
        {
            get => (CompetitionTypeEnum) CompetitionTypeId;
            set => CompetitionTypeId = (int) value;
        }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<ExtraPlayerInfo> ExtraPlayerInfos { get; set; }

        public virtual ICollection<Distance> Distances { get; set; }

        public virtual ICollection<AgeCategory> AgeCategories { get; set; }
    }
}