using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerCompetitionRegisterDto : IDtoBase<Player>
    {
        public PlayerCompetitionRegisterDto()
        {

        }

        public PlayerCompetitionRegisterDto(Player player)
        {
            Id = player.Id;
            FirstName = player.FirstName;
            LastName = player.LastName;
            BirthDate = player.BirthDate;
            IsMale = player.IsMale;
            Team = player.Team;
            City = player.City;
            Email = player.Email;
            PhoneNumber = player.PhoneNumber;
            ExtraData = player.ExtraData;
            SubcategoryId = player.Subcategory.Id;
            DistanceId = player.Distance.Id;
            CompetitionId = player.CompetitionId;
        }

        public Player CopyDataFromDto(Player player)
        {
            player.Id = Id;
            player.FirstName = FirstName;
            player.LastName = LastName;
            player.BirthDate = BirthDate;
            player.IsMale = IsMale;
            player.Team = Team;
            player.City = City;
            player.Email = Email;
            player.PhoneNumber = PhoneNumber;
            player.ExtraData = ExtraData;
            player.SubcategoryId = SubcategoryId;
            player.DistanceId = DistanceId;
            player.CompetitionId = CompetitionId;
            return player;
        }

        public async Task<Player> CopyDataFromDto(Player player, IContextProvider contextProvider, Competition competition)
        {
            SubcategoryRepository subcategoryRepository = new SubcategoryRepository(contextProvider, competition);
            DistanceRepository distanceRepository = new DistanceRepository(contextProvider, competition);

            CopyDataFromDto(player);
            player.Subcategory = await subcategoryRepository.GetById(SubcategoryId);
            player.Distance = await distanceRepository.GetById(DistanceId);

            if (player.Subcategory == null || player.Distance == null)
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

        public bool IsMale { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        public string ExtraData { get; set; }

        [StringLength(255)]
        public string Team { get; set; }

        public string City { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public int SubcategoryId { get; set; }

        public int DistanceId { get; set; }

        public int CompetitionId { get; set; }

        public string ReCaptchaToken { get; set; }
    }
}