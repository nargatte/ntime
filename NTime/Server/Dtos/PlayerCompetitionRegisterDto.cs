using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using BaseCore.DataBase;

namespace Server.Dtos
{
    public class PlayerCompetitionRegisterDto : PlayerListViewDto, IDtoBase<Player>
    {
        public PlayerCompetitionRegisterDto()
        {

        }

        public PlayerCompetitionRegisterDto(Player player) : base(player)
        {
            BirthDate = player.BirthDate;
            Email = player.Email;
            PhoneNumber = player.PhoneNumber;
            ExtraData = player.ExtraData;
            SubcategoryId = player.Subcategory.Id;
            DistanceId = player.Distance.Id;
            AgeCategoryId = player.AgeCategory.Id;
            ExtraColumnValues = player.ExtraColumnValues.Select(columnValue => new ExtraColumnValueDto(columnValue)).ToArray();
        }

        public override Player CopyDataFromDto(Player player)
        {
            base.CopyDataFromDto(player);
            player.Email = Email;
            player.BirthDate = BirthDate;
            player.PhoneNumber = PhoneNumber;
            player.ExtraData = ExtraData;
            player.SubcategoryId = SubcategoryId;
            player.DistanceId = DistanceId;
            player.AgeCategoryId = AgeCategoryId;
            player.ExtraColumnValues = ExtraColumnValues.Select(columnValue => columnValue.CopyDataFromDto(new ExtraColumnValue())).ToArray();
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

            if (player.Subcategory == null)
                throw new Exception($"Nie ma podkategorii o id {SubcategoryId} skorelowanego z zawodami o id {CompetitionId}.");

            if(player.Distance == null)
                throw new Exception($"Nie ma dystansu o id {DistanceId} skorelowanego z zawodami o id {CompetitionId}.");

            if (player.AgeCategory == null)
                throw new Exception($"Nie ma kategorii wiekowej o id {AgeCategoryId} skorelowanego z zawodami o id {CompetitionId}.");

            if (player.AgeCategory.Male != player.IsMale) 
                throw new Exception($"Płeć zawodnika nie zgadza się z płcią kategorii wiekowej {player.AgeCategory.Name}.");

            if (!(await ageCategoryRepository.IsAgeCategoryAndDistanceMatch(player.AgeCategory, player.Distance)))
                throw new Exception($"Kategoria wiekowa {player.AgeCategory.Name} i dystans {player.Distance.Name} nie są ze sobą skorelowane.");

            if ((await ageCategoryRepository.GetFittingAsync(player)).All(ac => ac.Id != player.AgeCategory.Id))
                throw new Exception(
                    $"Wybrana kategoria wiekowa {player.AgeCategory.Name} nie zawiera wieku zawodnika {player.BirthDate:d}, wiek powinien się zawierać w przedziale od {player.AgeCategory.YearFrom} do {player.AgeCategory.YearTo}");

            return player;
        }


        [Required]
        public DateTime BirthDate { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int SubcategoryId { get; set; }
        public int DistanceId { get; set; }
        public int AgeCategoryId { get; set; }
        public string ReCaptchaToken { get; set; }
    }
}