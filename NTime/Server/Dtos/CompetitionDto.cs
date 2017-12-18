﻿using System;
using System.ComponentModel.DataAnnotations;
using BaseCore.DataBase;

namespace BaseCore.Dtos
{
    public class CompetitionDto
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
            Link = competition.Link;
            Organiser = competition.Organiser;
            City = competition.City;
        }

        public Competition UpDateCompetition(Competition competition)
        {
            competition.Id = Id;
            competition.Name = Name;
            competition.EventDate = EventDate;
            competition.SignUpEndDate = SignUpEndDate;
            competition.Description = Description;
            competition.Link = Link;
            competition.Organiser = Organiser;
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

        [StringLength(2000)]
        public string Link { get; set; }

        [StringLength(255)]
        public string Organiser { get; set; }

        [StringLength(255)]
        public string City { get; set; }
    }
}