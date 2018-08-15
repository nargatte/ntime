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
    [RoutePrefix("api/OrganizerAccount")]
    public class OrganizerAccountController : ControllerNTimeBase
    {
        private readonly OrganizerAccountRepository _organizerAccountRepository;

        private ApplicationDbContext _applicationDBContext = new ApplicationDbContext();
        private UserManager<ApplicationUser> _userManager;

        protected OrganizerAccountController()
        {
            _organizerAccountRepository = new OrganizerAccountRepository(ContextProvider);
            _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_applicationDBContext));
        }

        // GET api/OrganizerAccount/Search/Ada?ItemsOnPage=10&PageNumber=0
        // GET api/OrganizerAccount/Search?ItemsOnPage=10&PageNumber=0
        [Route("Search/{query?}")]
        [Authorize(Roles = "Administrator")]
        public async Task<PageViewModel<OrganizerAccountDto>> GetSearch([FromUri] PageBindingModel pageBindingModel, string query = null)
        {
            PageViewModel<OrganizerAccount> viewModel = await _organizerAccountRepository.GetByQuery(query, pageBindingModel);
            PageViewModel<OrganizerAccountDto> viewModelDto = new PageViewModel<OrganizerAccountDto>
            {
                Items = viewModel.Items.Select(pa => new OrganizerAccountDto(pa)).ToArray(),
                TotalCount = viewModel.TotalCount
            };
            return viewModelDto;
        }

        // GET api/OrganizerAccount/ByCompetition/1
        [Route("ByCompetition/{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetByCompetition(int id)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            OrganizerAccount[] organizerAccounts =
                await _organizerAccountRepository.GetOrganizersByCompetition(Competition);

            return Ok(organizerAccounts.Select(o => new OrganizerAccountDto(o)).ToArray());
        }

        // GET api/OrganizerAccount/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Get(int id)
        {
            OrganizerAccount organizerAccount = await _organizerAccountRepository.GetById(id);

            if (organizerAccount == null)
                return NotFound();

            return Ok(new OrganizerAccountDto(organizerAccount)
            {
                CompetitionDtos = (await CompetitionRepository.GetCompetitionsByOrganizer(organizerAccount.AccountId))
                .Select(c => new CompetitionDto(c)).ToArray()
            });
        }

        // GET api/OrganizerAccount
        [Route()]
        [Authorize(Roles = "Organizer")]
        public async Task<IHttpActionResult> Get()
        {
            OrganizerAccount organizerAccount =
                await _organizerAccountRepository.GetByAccountId(User.Identity.GetUserId());

            if (organizerAccount == null)
                return NotFound();

            return Ok(new OrganizerAccountDto(organizerAccount)
            {
                CompetitionDtos = (await CompetitionRepository.GetCompetitionsByOrganizer(organizerAccount.AccountId))
                    .Select(c => new CompetitionDto(c)).ToArray()
            });
        }

        // PUT api/OrganizerAccount/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Put(int id, OrganizerAccountDto accountDto)
        {
            accountDto.Id = id;

            OrganizerAccount organizerAccount = await _organizerAccountRepository.GetById(id);

            if (organizerAccount == null)
                return NotFound();

            accountDto.CopyDataFromDto(organizerAccount);

            await _organizerAccountRepository.UpdateAsync(organizerAccount);
            return Ok();
        }

        // POST api/OrganizerAccount
        [Route]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(OrganizerAccountDto accountDto)
        {
            OrganizerAccount account = new OrganizerAccount();
            accountDto.CopyDataFromDto(account);

            var user = new ApplicationUser() { UserName = accountDto.Email, Email = accountDto.Email };

            string pass = PasswordGenerator();

            IdentityResult result = await _userManager.CreateAsync(user, pass);
            await _userManager.AddToRoleAsync(user.Id, "Organizer");

            account.AccountId = user.Id;
            await _organizerAccountRepository.AddAsync(account);
            accountDto.Id = account.Id;

            if (!result.Succeeded)
            {
                return InternalServerError();
            }

            return Created(Url.Content("~/api/OrganizerAccount/" + accountDto.Id), accountDto);
        }

        // DELETE api/OrganizerAccount/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            OrganizerAccount organizerAccount = await _organizerAccountRepository.GetById(id);

            if (organizerAccount == null)
                return NotFound();

            ApplicationUser user = await _userManager.FindByIdAsync(organizerAccount.AccountId);

            await _userManager.DeleteAsync(user);

            await _organizerAccountRepository.RemoveAsync(organizerAccount);

            return Ok();
        }

        // POST api/OrganizerAccount/1/PasswordReset
        [Route("{id:int:min(1)}/PasswordReset")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> PostPasswordReset(int id)
        {
            OrganizerAccount organizerAccount = await _organizerAccountRepository.GetById(id);

            if (organizerAccount == null)
                return NotFound();

            string password = PasswordGenerator();


            IdentityResult result = await _userManager.RemovePasswordAsync(organizerAccount.AccountId);
            IdentityResult result2 = await _userManager.AddPasswordAsync(organizerAccount.AccountId, password);

            if (!result.Succeeded && !result2.Succeeded)
            {
                return InternalServerError();
            }

            return Ok(new PasswordViewModel {Password = password});
        }

        // POST api/OrganizerAccount/1/SetCompetition/1
        [Route("{ido:int:min(1)}/SetCompetition/{idc:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> PostSetCompetition(int ido, int idc)
        {
            OrganizerAccount organizerAccount = await _organizerAccountRepository.GetById(ido);

            if (organizerAccount == null)
                return NotFound();

            if(await InitCompetitionById(idc) == false)
                return NotFound();

            await _organizerAccountRepository.SetCompetiton(organizerAccount, Competition);
            return Ok();
        }

        // POST api/OrganizerAccount/1/UnsetCompetition/1
        [Route("{ido:int:min(1)}/UnsetCompetition/{idc:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> PostUnsetCompetition(int ido, int idc)
        {
            OrganizerAccount organizerAccount = await _organizerAccountRepository.GetById(ido);

            if (organizerAccount == null)
                return NotFound();

            if (await InitCompetitionById(idc) == false)
                return NotFound();

            await _organizerAccountRepository.UnsetCompetiton(organizerAccount, Competition);
            return Ok();
        }

    }
}