﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using BaseCore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Dtos;
using Server.Models;

namespace Server.Controllers
{
    [RoutePrefix("api/player")]
    public class PlayerController : ControllerNTimeBase
    {
        private PlayerRepository _playerRepository;

        protected override async Task<bool> InitCompetitionByRelatedEntitieId<T>(int id)
        {
            if (await base.InitCompetitionByRelatedEntitieId<T>(id) == false)
                return false;
            _playerRepository = new PlayerRepository(ContextProvider, Competition);
            return true;
        }

        protected override async Task<bool> InitCompetitionById(int Id)
        {
            if (await base.InitCompetitionById(Id) == false)
                return false;
            _playerRepository = new PlayerRepository(ContextProvider, Competition);
            return true;
        }

        protected async Task<bool> CanPlayerAccess(Player player)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(User.Identity.GetUserId());
            if (s[0] == "Player" &&
                ! await _playerRepository.CanPlayerEdit(User.Identity.GetUserId(), player))
                return false;
            return true;
        }

        protected bool AmIPlayer() =>
            AmI("Player");

        // POST api/Player/TakeSimpleList/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        [Route("takeSimpleList/FromCompetition/{id:int:min(1)}")]
        public async Task<IHttpActionResult> PostTakeSimpleListFromCompetition(int id, [FromUri]PageBindingModel bindingModel,
            PlayerFilterOptionsBindingModel filterOptions)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            PageViewModel<Player> pagePlayer = await _playerRepository.GetAllByFilterAsync(filterOptions.CreatePlayerFilterOptions(), bindingModel);
            return Ok(new PageViewModel<PlayerListViewDto>
            {
                Items = pagePlayer.Items.Select(p => new PlayerListViewDto(p)).ToArray(),
                TotalCount = pagePlayer.TotalCount
            });
        }

        // POST api/Player/TakeFullList/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        [Authorize(Roles = "Administrator,Organizer,Moderator")]
        [Route("takeFullList/FromCompetition/{id:int:min(1)}")]
        public async Task<IHttpActionResult> PostTakeFullListFromCompetition(int id, [FromUri]PageBindingModel bindingModel,
            PlayerFilterOptionsBindingModel filterOptions)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            PageViewModel<Player> pagePlayer = await _playerRepository.GetAllByFilterAsync(filterOptions.CreatePlayerFilterOptions(), bindingModel);
            return Ok(new PageViewModel<PlayerWithScoresDto>
            {
                Items = pagePlayer.Items.Select(p => new PlayerWithScoresDto(p)).ToArray(),
                TotalCount = pagePlayer.TotalCount
            });
        }

        // GET api/Player/TakeFull/FromPlayerAccount/1?ItemsOnPage=10&PageNumber=0
        [Route("takeFull/FromPlayerAccount/{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Moderator,Player")]
        public async Task<IHttpActionResult> GetTakeFullFromPlayerAccount([FromUri]PageBindingModel bindingModel, int id)
        {
            PlayerAccountRepository accountRepository = new PlayerAccountRepository(ContextProvider);

            PlayerAccount playerAccount = await accountRepository.GetById(id);

            if (CanPlayerAccess(playerAccount) == false)
                return Unauthorized();

            PageViewModel<Player> pagePlayer =
                await accountRepository.GetPlayersByPlayerAccount(playerAccount.AccountId, bindingModel);

            return Ok(new PageViewModel<PlayerWithScoresDto>
            {
                Items = pagePlayer.Items.Select(p => new PlayerWithScoresDto(p)).ToArray(),
                TotalCount = pagePlayer.TotalCount
            });
        }

        // GET api/Player/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Organizer,Moderator,Player")]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Player player = await _playerRepository.GetById(id);

            if (await CanPlayerAccess(player) == false)
                return Unauthorized();

            return Ok(new PlayerWithScoresDto(player));
        }

        // PUT api/Player/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Organizer,Moderator")]
        public async Task<IHttpActionResult> Put(int id, PlayerWithScoresDto playerWithScoresDto)
        {
            playerWithScoresDto.Id = id;

            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Player player = await _playerRepository.GetById(id);

            await playerWithScoresDto.CopyDataFromDto(player, ContextProvider, Competition);

            await _playerRepository.UpdateAsync(player, player.Distance, player.ExtraPlayerInfo);

            return Ok();
        }

        // DELETE api/Player/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Organizer,Moderator,Player")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Player player = await _playerRepository.GetById(id);

            if (await CanPlayerAccess(player) == false)
                return Unauthorized();

            await _playerRepository.RemoveAsync(player);

            return Ok();
        }

        // GET api/Player/Register/1
        [Route("register/{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Organizer,Moderator,Player")]
        public async Task<IHttpActionResult> GetRegister(int id)
        {
            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Player player = await _playerRepository.GetById(id);

            if (await CanPlayerAccess(player) == false)
                return Unauthorized();

            return Ok(new PlayerCompetitionRegisterDto(player));
        }

        // PUT api/Player/Register/1
        [Route("register/{id:int:min(1)}")]
        [Authorize(Roles = "Player")]
        public async Task<IHttpActionResult> PutRegister(int id, PlayerCompetitionRegisterDto competitionRegisterDto)
        {
            competitionRegisterDto.Id = id;

            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (Competition.SignUpEndDate != null && Competition.SignUpEndDate < DateTime.Now)
                return Conflict();

            Player player = await _playerRepository.GetById(id);

            if (await CanPlayerAccess(player) == false)
                return Unauthorized();

            if (await competitionRegisterDto.CopyDataFromDto(player, ContextProvider, Competition) == null)
                return NotFound();

            await _playerRepository.UpdateAsync(player, player.Distance, player.ExtraPlayerInfo);

            return Ok();
        }

        // POST api/Player/Register/IntoCompetition/1
        [Route("register/intocompetition/{id:int:min(1)}")]
        public async Task<IHttpActionResult> PostRegisterIntoCompetition(int id,
            PlayerCompetitionRegisterDto competitionRegisterDto)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false && AmI("Administrator") == false && AmI("Moderator") == false && Competition.SignUpEndDate != null && Competition.SignUpEndDate < DateTime.Now)
                return Conflict();

            if (AmIPlayer() && await _playerRepository.IsNowRegister(User.Identity.GetUserId()))
                return Conflict();

            Player player = new Player();
            if (await competitionRegisterDto.CopyDataFromDto(player, ContextProvider, Competition) == null)
                return NotFound();

            if (AmIPlayer())
            {
                PlayerAccountRepository accountRepository = new PlayerAccountRepository(ContextProvider);
                player.PlayerAccount = await accountRepository.GetByAccountId(User.Identity.GetUserId());
                player.PlayerAccountId = player.PlayerAccount.Id;
            }

            await _playerRepository.AddAsync(player, player.Distance, player.ExtraPlayerInfo);

            competitionRegisterDto.Id = player.Id;
            return Created(Url.Content("~/player/register/" + competitionRegisterDto.Id), competitionRegisterDto);
        }
    }
}