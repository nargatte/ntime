using System;
using System.ComponentModel.DataAnnotations;
using BaseCore.DataBase;
using Server.Dtos;
using Server.Models;

namespace Server.Dtos
{
    public class CompetitionDto : IDtoBase<Competition>
    {
        public CompetitionDto()
        {
        }

        public CompetitionDto(Competition competition)
        {
            Id = competition.Id;
            Name = competition.Name;
            EventDate = competition.EventDate;
            SignUpEndDate = competition.SignUpEndDate;
            Description = competition.Description;
            ExtraDataHeaders = competition.ExtraDataHeaders;
            SubcategoryAlternativeName = competition.SubcategoryAlternativeName;
            Link = competition.Link;
            Organizer = competition.Organizer;
            City = competition.City;
            OrganizerEditLock = competition.OrganizerEditLock;
        }

        public Competition CopyDataFromDto(Competition competition)
        {
            competition.Id = Id;
            competition.Name = Name;
            competition.EventDate = EventDate;
            competition.SignUpEndDate = SignUpEndDate;
            competition.Description = Description;
            competition.ExtraDataHeaders = ExtraDataHeaders;
            competition.SubcategoryAlternativeName = SubcategoryAlternativeName;
            competition.Link = Link;
            competition.Organizer = Organizer;
            competition.City = City;
            return competition;
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public DateTime EventDate { get; set; }

        public DateTime? SignUpEndDate { get; set; }

        public string Description { get; set; }

        public string ExtraDataHeaders { get; set; }

        public string SubcategoryAlternativeName { get; set; }

        [StringLength(2000)]
        public string Link { get; set; }

        [StringLength(255)]
        public string Organizer { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        public bool OrganizerEditLock { get; set; }
    }
}