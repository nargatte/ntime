using System;
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

            return Ok(new PlayerAccountDto(playerAccount));
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

            return Ok(new PlayerAccountDto(playerAccount));
        }

        // GET api/PlayerAccount/FromCompetition/1?ItemsOnPage=10&PageNumber=0
        [Route("FromCompetition/{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetFromCompetition([FromUri] PageBindingModel pageBindingModel, int id)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            PageViewModel<PlayerAccount> viewModel =
                await _playerAccountRepository.GetPlayersAccountsByCompetition(Competition, pageBindingModel);
            PageViewModel<PlayerAccountDto> viewModelDto = new PageViewModel<PlayerAccountDto>
            {
                Items = viewModel.Items.Select(pa => new PlayerAccountDto(pa)).ToArray(),
                TotalCount = viewModel.TotalCount
            };
            return Ok(viewModelDto);
        }

        // PUT api/PlayerAccount/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator,Player")]
        public async Task<IHttpActionResult> Put(int id, PlayerAccountDto accountDto)
        {
            accountDto.Id = id;

            PlayerAccount playerAccount = await _playerAccountRepository.GetById(id);

            if (playerAccount == null)
                return NotFound();

            if (CanPlayerAccess(playerAccount) == false)
                return Unauthorized();

            accountDto.CopyDataFromDto(playerAccount);

            await _playerAccountRepository.UpdateAsync(playerAccount);
            return Ok();
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