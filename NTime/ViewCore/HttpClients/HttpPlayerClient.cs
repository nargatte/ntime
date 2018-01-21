using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.Models;
using BaseCore.PlayerFilter;
using Server.Dtos;
using Server.Models;

namespace ViewCore.HttpClients
{
    public class HttpPlayerClient : HttpClientBase
    {
        public HttpPlayerClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        protected PlayerWithScoresDto CreatePlayerWithScoresDto(Player entity)
            => new PlayerWithScoresDto(entity);

        protected PlayerCompetitionRegisterDto CreatePlayerCompetitionRegisterDto(Player entity)
            => new PlayerCompetitionRegisterDto(entity);

        protected PlayerListViewDto CreatePlayerListViewDto(Player entity)
            => new PlayerListViewDto(entity);

        protected string GetFromCompetitionQuery(int competitionId)
        {
            return $"TakeFullList/FromPlayerAccount/{competitionId.ToString()}";
        }

        //POST api/Player/TakeSimpleList/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        public async Task<PageViewModel<PlayerListViewDto>> GetPlayersListView(Competition competition,
            int itemsOnPage, int pageNumber, PlayerFilterOptions filterOptions)
        {
            return await base.PostAsync<PlayerFilterOptionsBindingModel, PageViewModel<PlayerListViewDto>>(
                $"TakeSimpleList/FromCompetition/{competition.Id}{base.GetPageQuery(itemsOnPage, pageNumber)}",
                new PlayerFilterOptionsBindingModel(filterOptions));
        }


        //POST api/Player/TakeFullList/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        public async Task<PageViewModel<PlayerWithScoresDto>> GetPlayersWithScore(Competition competition,
            int itemsOnPage, int pageNumber, PlayerFilterOptions filterOptions)
        {
            return await base.PostAsync<PlayerFilterOptionsBindingModel, PageViewModel<PlayerWithScoresDto>>(
                $"TakeFullList/FromCompetition/{competition.Id}{base.GetPageQuery(itemsOnPage, pageNumber)}",
                new PlayerFilterOptionsBindingModel(filterOptions));
        }

        //GET api/Player/TakeFullList/FromPlayerAccount/1?ItemsOnPage=10&PageNumber=0
        public async Task<PageViewModel<PlayerWithScoresDto>> GetPlayersFromTheiCompetitions(PlayerAccount playerAccount,
            int itemsOnPage, int pageNumber, PlayerFilterOptions filterOptions)
        {
            return await base.PostAsync<PlayerFilterOptionsBindingModel, PageViewModel<PlayerWithScoresDto>>(
                $"TakeFullList/FromPlayerAccount/{playerAccount.Id}{base.GetPageQuery(itemsOnPage, pageNumber)}",
                new PlayerFilterOptionsBindingModel(filterOptions));
        }

        //GET api/player/FromPlayerAccount/{idpa}/FromCompetition/{idc}
        public async Task<PlayerCompetitionRegisterDto> GetFullRegisteredPlayerFromCompetition(Competition competition, PlayerAccount playerAccount)
        {
            return await base.GetAsync<PlayerCompetitionRegisterDto>($"FromPlayerAccount/{playerAccount.Id}/FromCompetition/{competition.Id}");
        }

        public async Task<PlayerWithScoresDto> GetPlayerInfo(int playerId)
        {
            return await base.GetAsync<PlayerWithScoresDto>(playerId.ToString());
        }

        public async Task UpdatePlayerFullInfo(Player player)
        {
            var playerDto = CreatePlayerWithScoresDto(player);
            await base.PutAsync<PlayerWithScoresDto>(playerDto.Id.ToString(), playerDto);
        }

        public async Task UpdatePlayerRegisterInfo(Player player)
        {
            var playerDto = CreatePlayerCompetitionRegisterDto(player);
            await base.PutAsync<PlayerCompetitionRegisterDto>($"Register/{playerDto.Id.ToString()}", playerDto);
        }

        public async Task Delete(Player player)
        {
            await base.DeleteAsync(player.Id.ToString());
        }

        public async Task<PlayerCompetitionRegisterDto> GetRegisteredPlayer(int playerId)
        {
            return await base.GetAsync<PlayerCompetitionRegisterDto>($"Register/{playerId.ToString()}");
        }

        public async Task<PlayerCompetitionRegisterDto> AddRegisteredPlayer(int competitionId, Player player)
        {
            var playerDto = CreatePlayerCompetitionRegisterDto(player);
            return await base.PostAsync<PlayerCompetitionRegisterDto, PlayerCompetitionRegisterDto>($"Register/IntoCompetition/{competitionId.ToString()}", playerDto);
        }

    }
}
