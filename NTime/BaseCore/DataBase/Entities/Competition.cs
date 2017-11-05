using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BaseCore.DataBase
{
    public class Competition
    {
        public Competition()
        {
        }

        public Competition(string name, DateTime eventDate, string description, string link, string organiser, string city, int competitionTypeId)
        {
            Name = name;
            EventDate = eventDate;
            Description = description;
            Link = link;
            Organiser = organiser;
            City = city;
            CompetitionTypeId = competitionTypeId;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public string Description { get; set; }

        [StringLength(2000)]
        public string Link { get; set; }

        [StringLength(256)]
        public string Organiser { get; set; }

        [StringLength(256)]
        public string City { get; set; }

        public int CompetitionTypeId { get; set; }
        public virtual CompetitionType CompetitionType { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<ExtraPlayerInfo> ExtraPlayerInfos { get; set; }

        public virtual ICollection<Distance> Distances { get; set; }

        public virtual ICollection<AgeCategory> AgeCategories { get; set; }
    }
}