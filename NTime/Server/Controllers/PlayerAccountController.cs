using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using BaseCore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Server.Dtos;
using Server.Models;

namespace Server.Controllers
{
    [RoutePrefix("api/PlayerAccount")]
    public class PlayerAccountController : ControllerNTimeBase
    {
        private PlayerAccountRepository _playerAccountRepository;

        protected PlayerAccountController()
        {
            _playerAccountRepository = new PlayerAccountRepository(ContextProvider);
        }

        // GET api/PlayerAccount/Search/Ada?ItemsOnPage=10&PageNumber=0
        // GET api/PlayerAccount/Search?ItemsOnPage=10&PageNumber=0
        [Route("Search/{query?}")]
        [Authorize(Roles = "Administrator")]
        public async Task<PageViewModel<PlayerAccountDto>> GetSearch([FromUri] PageBindingModel pageBindingModel, string query = null)
        {
            //PageViewModel<PlayerAccount> viewModel = await _playerAccountRepository.GetByQuery(query, pageBindingModel);
            //PageViewModel<PlayerAccountDto> viewModelDto = new PageViewModel<PlayerAccountDto>
            //{
            //    Items = viewModel.Items.Select(pa => new PlayerAccountDto(pa)).ToArray(),
            //    TotalCount = viewModel.TotalCount
            //};
            //return viewModelDto;
            throw new NotImplementedException();
        }

        // GET api/PlayerAccount
        [Route]
        [Authorize(Roles = "Player")]
        public async Task<IHttpActionResult> Get()
        {
            PlayerAccount playerAccount = await _playerAccountRepository.GetByAccountId(User.Identity.GetUserId());

            if (playerAccount == null)
                return NotFound();

            if (CanPlayerAccess(playerAccount) == false)
                return Unauthorized();

            var accountRepository = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userAccount = await accountRepository.FindByIdAsync(User.Identity.GetUserId());

            return Ok(new PlayerAccountDto(playerAccount, userAccount));
        }

        // GET api/PlayerAccount/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Player")]
        public async Task<IHttpActionResult> Get(int id)
        {
            PlayerAccount playerAccount = await _playerAccountRepository.GetById(id);

            if (playerAccount == null)
                return NotFound();

            if (CanPlayerAccess(playerAccount) == false)
                return Unauthorized();

            var accountRepository = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userAccount = await accountRepository.FindByIdAsync(playerAccount.AccountId);

            return Ok(new PlayerAccountDto(playerAccount, userAccount));
        }

        // GET api/PlayerAccount/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        [Route("FromCompetition/{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetFromCompetition([FromUri] PageBindingModel pageBindingModel, int id)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            var accountRepository = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();

            PageViewModel<PlayerAccount> viewModel =
                await _playerAccountRepository.GetPlayersAccountsByCompetition(Competition, pageBindingModel);

            var playerAccounts = new List<PlayerAccountDto>();
            foreach (var pa in viewModel.Items)
            {
                var userAccount = await accountRepository.FindByIdAsync(pa.AccountId);
                playerAccounts.Add(new PlayerAccountDto(pa, userAccount));
            }
            PageViewModel<PlayerAccountDto> viewModelDto = new PageViewModel<PlayerAccountDto>
            {
                Items = playerAccounts.ToArray(),
                TotalCount = viewModel.TotalCount
            };
            return Ok(viewModelDto);
        }

        // PUT api/PlayerAccount/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Player")]
        public /*async*/ Task<IHttpActionResult> Put(int id, PlayerAccountDto accountDto)
        {
            //accountDto.Id = id;

            //var playerAccount = await _playerAccountRepository.GetById(id);

            //if (playerAccount == null)
            //    return NotFound();

            //if (CanPlayerAccess(playerAccount) == false)
            //    return Unauthorized();

            //// TODO: We need to be able to Include ApplicationUser to the playerAccount
            //var userAccount = _

            //accountDto.CopyDataFromDto(playerAccount);

            //await _playerAccountRepository.UpdateAsync(playerAccount);
            //return Ok();
            throw new NotImplementedException();
        }

        //public async Task<IHttpActionResult> Delete(int id)
        //{
        //    PlayerAccount playerAccount = await _playerAccountRepository.GetById(id);

        //    if (playerAccount == null)
        //        return NotFound();

        //    await _playerAccountRepository.RemoveAsync(playerAccount);

        //    ApplicationDbContext context = new ApplicationDbContext();
        //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

        //    IdentityResult result = await UserManager.RemovePasswordAsync(playerAccount.AccountId);

        //    if (!result.Succeeded)
        //    {
        //        throw new Exception(result.ToString());
        //    }

        //    return Ok();
        //}
    }
}