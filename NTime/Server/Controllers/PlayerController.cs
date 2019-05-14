using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using BaseCore.DataBase;
using BaseCore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using NTime.Application.Exceptions;
using Server.Dtos;
using Server.Models;

namespace Server.Controllers
{
    [System.Web.Http.RoutePrefix("api/player")]
    public class PlayerController : ControllerNTimeBase
    {
        private const string _registerPrivateKey = "6LfUoFAUAAAAAEGYb9BcgluK0dDdqqhTTxieoVGU";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://www.google.com/")
        };

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
                !await _playerRepository.CanPlayerEdit(User.Identity.GetUserId(), player))
                return false;
            return true;
        }

        protected bool AmIPlayer() =>
            AmI("Player");

        private async Task CheckReCaptcha(string privateKey, string token)
        {
            if (token.ToLower().Contains("android"))
                return;

            Dictionary<string, string> postData = new Dictionary<string, string>
            {
                {"secret", _registerPrivateKey},
                {"response", token},
                {"remoteip", HttpContext.Current.Request.UserHostAddress},
            };

            ReCaptchaResponseModel reCaptchaResponseModel = null;

            using (var httpClient = new HttpClient { BaseAddress = new Uri("https://www.google.com/") })
            {
                using (var content = new FormUrlEncodedContent(postData))
                {
                    content.Headers.Clear();
                    content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                    HttpResponseMessage response = await httpClient.PostAsync("recaptcha/api/siteverify", content);

                    response.EnsureSuccessStatusCode();

                    reCaptchaResponseModel = await response.Content.ReadAsAsync<ReCaptchaResponseModel>();

                }
            }
            if (reCaptchaResponseModel.Success == false)
                throw new Exception("ReCaptcha problem: " + reCaptchaResponseModel.ErrorCodes.ToString());
        }

        // POST api/Player/TakeSimpleList/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        [System.Web.Http.Route("takeSimpleList/FromCompetition/{id:int:min(1)}")]
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
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator")]
        [System.Web.Http.Route("takeFullList/FromCompetition/{id:int:min(1)}")]
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

        // GET api/Player/TakeFullList/FromPlayerAccount/1?ItemsOnPage=10&PageNumber=0
        [System.Web.Http.Route("takeFullList/FromPlayerAccount/{id:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Administrator,Moderator,Player")]
        public async Task<IHttpActionResult> GetTakeFullListFromPlayerAccount([FromUri]PageBindingModel bindingModel, int id)
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

        // GET api/Player/FromPlayerAccount/1/FromCompetition/1
        [System.Web.Http.Route("FromPlayerAccount/{idpa:int:min(1)}/FromCompetition/{idc:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Administrator,Moderator,Player")]
        public async Task<IHttpActionResult> GetFromPlayerAccountFromCompetition(int idpa, int idc)
        {
            if (await InitCompetitionById(idc) == false)
                return NotFound();

            PlayerAccountRepository accountRepository = new PlayerAccountRepository(ContextProvider);

            PlayerAccount playerAccount = await accountRepository.GetById(idpa);

            if (CanPlayerAccess(playerAccount) == false)
                return Unauthorized();

            Player player = await _playerRepository.GetFromPlayerAccountFromCompetition(playerAccount, Competition);
            if (player == null)
                return NotFound();

            return Ok(new PlayerWithScoresDto(player));
        }

        // GET api/Player/1
        [System.Web.Http.Route("{id:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator,Player")]
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
        [System.Web.Http.Route("{id:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator")]
        [System.Web.Http.HttpPut]
        public async Task<IHttpActionResult> Put(int id, PlayerWithScoresDto playerWithScoresDto)
        {
            playerWithScoresDto.Id = id;

            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (await CanOrganizerAccessAndEdit() == false)
                return Unauthorized();

            Player player = await _playerRepository.GetById(id);

            await playerWithScoresDto.CopyDataFromDto(player, ContextProvider, Competition);

            await _playerRepository.UpdateAsync(player, player.AgeCategory, player.Distance, player.Subcategory, player.ExtraColumnValues.ToArray());

            return Ok();
        }

        // PUT api/Player
        [System.Web.Http.Route]
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator")]
        [System.Web.Http.HttpPut]
        public async Task<IHttpActionResult> Put(PlayerWithScoresDto[] playerWithScoresDtos)
        {
            foreach (PlayerWithScoresDto p in playerWithScoresDtos)
            {
                if (await InitCompetitionByRelatedEntitieId<Player>(p.Id) == false)
                    return NotFound();

                if (await CanOrganizerAccessAndEdit() == false)
                    return Unauthorized();

                Player player = await _playerRepository.GetById(p.Id);

                await p.CopyDataFromDto(player, ContextProvider, Competition);

                await _playerRepository.UpdateAsync(player, player.AgeCategory, player.Distance, player.Subcategory, player.ExtraColumnValues.ToArray());
            }

            return Ok();
        }

        // DELETE api/Player/1
        [System.Web.Http.Route("{id:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator,Player")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            if (await InitCompetitionByRelatedEntitieId<Player>(id) == false)
                return NotFound();

            if (await CanOrganizerAccessAndEdit() == false)
                return Unauthorized();

            Player player = await _playerRepository.GetById(id);

            if (await CanPlayerAccess(player) == false)
                return Unauthorized();

            await _playerRepository.RemoveAsync(player);

            return Ok();
        }

        // GET api/Player/Register/1
        [System.Web.Http.Route("register/{id:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator,Player")]
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
        [System.Web.Http.Route("register/{id:int:min(1)}")]
        [System.Web.Http.Authorize(Roles = "Player")]
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

            await competitionRegisterDto.CopyDataFromDto(player, ContextProvider, Competition);

            await _playerRepository.UpdateAsync(player, player.AgeCategory, player.Distance, player.Subcategory, player.ExtraColumnValues.ToArray());

            return Ok();
        }

        // POST api/Player/Register/IntoCompetition/1
        [System.Web.Http.Route("register/intocompetition/{id:int:min(1)}")]
        public async Task<IHttpActionResult> PostRegisterIntoCompetition(int id,
            PlayerCompetitionRegisterDto competitionRegisterDto)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            if (User.Identity.IsAuthenticated == true)
                if (await CanOrganizerAccess() == false && AmI("Administrator") == false && AmI("Moderator") == false)
                    return Conflict();

            if (Competition.SignUpEndDate != null && Competition.SignUpEndDate < DateTime.Now)
                return Conflict();

            if (AmIPlayer() && await _playerRepository.IsNowRegister(User.Identity.GetUserId()))
                return Conflict();

            Player player = new Player();
            try
            {
                await competitionRegisterDto.CopyDataFromDto(player, ContextProvider, Competition);
            }
            catch (CustomHttpException ex)
            {
                return BadRequest(ex.Message);
            }

            await CheckReCaptcha(_registerPrivateKey, competitionRegisterDto.ReCaptchaToken);

            if (AmIPlayer())
            {
                PlayerAccountRepository accountRepository = new PlayerAccountRepository(ContextProvider);
                player.PlayerAccount = await accountRepository.GetByAccountId(User.Identity.GetUserId());
                player.PlayerAccountId = player.PlayerAccount.Id;
                player.PlayerAccount = null;
            }

            await _playerRepository.AddAsync(player, player.AgeCategory, player.Distance, player.Subcategory);

            competitionRegisterDto.Id = player.Id;
            return Created(Url.Content("~/player/register/" + competitionRegisterDto.Id), competitionRegisterDto);
        }

        // GET api/Player/ExportFromCompetition/1
        [System.Web.Http.Authorize(Roles = "Administrator,Organizer,Moderator")]
        [System.Web.Http.Route("ExportFromCompetition/{id:int:min(1)}")]
        public async Task<HttpResponseMessage> GetExportFromCompetition(int id)
        {
            if (await InitCompetitionById(id) == false)
                throw new Exception("Invalid competition id");

            if (await CanOrganizerAccess() == false)
                throw new Exception("Unauthorized");

            var players = await _playerRepository.GetAllAsync();
            var stream = _playerRepository.ExportPlayersToCsv(players);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(stream)
            };
            result.Content.Headers.ContentDisposition =
                new ContentDispositionHeaderValue("attachment")
                {
                    FileName =
                "zawodnicy " + DateTime.Now.ToString("d") + " " + DateTime.Now.ToString("t") + ".csv"
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            return result;

        }
    }
}