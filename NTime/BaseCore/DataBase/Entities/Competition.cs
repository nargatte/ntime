using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BaseCore.DataBase
{
    public class Competition : IEntityId
    {
        public Competition()
        {
        }

        public Competition(string name, DateTime eventDate, string description, string link, string organizer, string city, string linkDisplayedName = "Link")
        {
            Name = name;
            EventDate = eventDate;
            Description = description;
            Link = link;
            Organizer = organizer;
            City = city;
            LinkDisplayedName = linkDisplayedName;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime? SignUpEndDate { get; set; }

        public string Description { get; set; }

        public string ExtraDataHeaders { get; set; }


        [StringLength(2000)]
        public string Link { get; set; }

        public string LinkDisplayedName { get; set; }

        [StringLength(255)]
        public string Organizer { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        public bool OrganizerEditLock { get; set; }

        public virtual ICollection<Player> Players { get; set; }

        public virtual ICollection<Subcategory> Subcategories { get; set; }

        public virtual ICollection<Distance> Distances { get; set; }

        public virtual ICollection<AgeCategory> AgeCategories { get; set; }

        public virtual ICollection<OrganizerAccount> OrganizerAccounts { get; set; }

        public ICollection<ExtraColumn> ExtraColumns { get; set; }
    }
}