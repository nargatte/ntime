using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerWithScoresDto : IDtoBase<Player>
    {
        public PlayerWithScoresDto()
        {

        }

        public PlayerWithScoresDto(Player player)
        {
            Id = player.Id;
            FirstName = player.FirstName;
            LastName = player.LastName;
            BirthDate = player.BirthDate;
            RegistrationDate = player.RegistrationDate;
            IsMale = player.IsMale;
            PhoneNumber = player.PhoneNumber;
            ExtraData = player.ExtraData;
            Team = player.Team;
            StartNumber = player.StartNumber;
            StartTime = player.StartTime;
            City = player.City;
            IsPaidUp = player.IsPaidUp;
            Email = player.Email;
            IsStartTimeFromReader = player.IsStartTimeFromReader;
            FullCategory = player.FullCategory;
            IsCategoryFixed = player.IsCategoryFixed;
            LapsCount = player.LapsCount;
            Time = player.Time;
            DistancePlaceNumber = player.DistancePlaceNumber;
            CategoryPlaceNumber = player.CategoryPlaceNumber;
            CompetitionCompleted = player.CompetitionCompleted;
            SubcategoryId = player.Subcategory.Id;
            DistanceId = player.Distance.Id;
            AgeCategoryId = player.AgeCategory.Id;
            PlayerAccountId = player.PlayerAccountId;
        }

        public Player CopyDataFromDto(Player player)
        {
            player.Id = Id;
            player.FirstName = FirstName;
            player.LastName = LastName;
            player.BirthDate = BirthDate;
            player.IsMale = IsMale;
            player.PhoneNumber = PhoneNumber;
            player.ExtraData = ExtraData;
            player.Team = Team;
            player.StartNumber = StartNumber;
            player.StartTime = StartTime;
            player.City = City;
            player.IsPaidUp = IsPaidUp;
            player.Email = Email;
            player.IsStartTimeFromReader = IsStartTimeFromReader;
            player.FullCategory = FullCategory;
            player.IsCategoryFixed = IsCategoryFixed;
            player.LapsCount = LapsCount;
            player.Time = Time;
            player.DistancePlaceNumber = DistancePlaceNumber;
            player.CategoryPlaceNumber = CategoryPlaceNumber;
            player.CompetitionCompleted = CompetitionCompleted;
            player.SubcategoryId = SubcategoryId;
            player.DistanceId = DistanceId;
            player.AgeCategoryId = AgeCategoryId;
            player.PlayerAccountId = PlayerAccountId;
            return player;
        }

        public async Task<Player> CopyDataFromDto(Player player, IContextProvider contextProvider, Competition competition)
        {
            SubcategoryRepository subcategoryRepository = new SubcategoryRepository(contextProvider, competition);
            DistanceRepository distanceRepository = new DistanceRepository(contextProvider, competition);
            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(contextProvider, competition);

            CopyDataFromDto(player);
            player.Subcategory = await subcategoryRepository.GetById(SubcategoryId);
            player.Distance = await distanceRepository.GetById(DistanceId);
            player.AgeCategory = await ageCategoryRepository.GetById(AgeCategoryId);

            if (player.Subcategory == null || player.Distance == null || player.AgeCategory == null)
                return null;

            if (player.AgeCategory.Male != player.IsMale) return null;
            if (!(await ageCategoryRepository.IsAgeCategoryAndDistanceMatch(player.AgeCategory, player.Distance)))
                return null;

            return player;
        }


        public int Id { get; set; }

        [StringLength(255), Required]
        public string FirstName { get; set; }

        [StringLength(255), Required]
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsMale { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string ExtraData { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        public int StartNumber { get; set; }

        public DateTime? StartTime { get; set; }

        public string City { get; set; }

        public bool IsPaidUp { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsStartTimeFromReader { get; set; }

        [StringLength(255)]
        public string FullCategory { get; set; }

        public bool IsCategoryFixed { get; set; }

        public int LapsCount { get; set; }

        public decimal Time { get; set; }

        public int DistancePlaceNumber { get; set; }

        public int CategoryPlaceNumber { get; set; }

        public bool CompetitionCompleted { get; set; }

        public int SubcategoryId { get; set; }

        public int DistanceId { get; set; }

        public int AgeCategoryId { get; set; }

        public int? PlayerAccountId { get; set; }
    }
}