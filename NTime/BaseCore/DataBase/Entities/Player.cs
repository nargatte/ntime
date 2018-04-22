using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BaseCore.TimesProcess;

namespace BaseCore.DataBase
{
    public class Player : IEntityId, ICompetitionId
    {
        public Player()
        {
        }

        public Player(string firstName, string lastName, DateTime birthDate, bool isMale, string team, int startNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            IsMale = isMale;
            Team = team;
            StartNumber = startNumber;
        }

        public Player(PlayerAccount playerAccount, string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = playerAccount.BirthDate ?? DateTime.Today;
            PhoneNumber = phoneNumber;
            Team = playerAccount.Team;
            if (playerAccount.IsMale.HasValue)
            {
                IsMale = playerAccount.IsMale.Value;
            }
        }

        public int Id { get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public bool IsMale { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        public string City { get; set; }

        public int StartNumber { get; set; }

        public DateTime? StartTime { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        public bool IsPaidUp { get; set; }

        public bool IsStartTimeFromReader { get; set; }

        [StringLength(255)]
        public string FullCategory { get; set; }

        public bool IsCategoryFixed { get; set; }

        public int LapsCount { get; set; }

        public decimal Time { get; set; }

        public int DistancePlaceNumber { get; set; }

        public int CategoryPlaceNumber { get; set; }

        public bool CompetitionCompleted { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string ExtraData { get; set; }

        public int CompetitionId { get; set; }
        public virtual Competition Competition { get; set; }

        public int? SubcategoryId { get; set; }
        public virtual Subcategory Subcategory { get; set; }

        public int? DistanceId { get; set; }
        public virtual Distance Distance { get; set; }

        public int? AgeCategoryId { get; set; }
        public virtual AgeCategory AgeCategory { get; set; }

        public int? PlayerAccountId { get; set; }
        public virtual PlayerAccount PlayerAccount { get; set; }

        public virtual ICollection<TimeRead> TimeReads { get; set; }

        public override string ToString()
        {
            return $"{nameof(FirstName)}: {FirstName}, {nameof(LastName)}: {LastName}, {nameof(IsMale)}: {IsMale}, {nameof(StartNumber)}: {StartNumber}, {nameof(StartTime)}: {StartTime}, {nameof(LapsCount)}: {LapsCount}, {nameof(Time)}: {Time.ToDateTime()}, {nameof(FullCategory)}: {FullCategory}";
        }
    }
}