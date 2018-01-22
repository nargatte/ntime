using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;
using BaseCore.DataBase;
using BaseCore.Models;
using BaseCoreTests;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Dtos;
using Server.Models;

namespace Server.Controllers
{
    [RoutePrefix("api/Competition")]
    public class CompetitionController : ControllerNTimeBase
    {

        // GET /api/Competition?ItemsOnPage=10&PageNumber=0
        [Route]
        public async Task<PageViewModel<CompetitionDto>> Get([FromUri]PageBindingModel pageBindingModel)
        {
            PageViewModel<Competition> pageViewModel = await CompetitionRepository.GetAllAsync(pageBindingModel);
            PageViewModel<CompetitionDto> pageViewModelDto = new PageViewModel<CompetitionDto>
            {
                TotalCount = pageViewModel.TotalCount,
                Items = pageViewModel.Items.Select(i => new CompetitionDto(i)).ToArray()
            };
            return pageViewModelDto;
        }

        // GET /api/Competition/1
        [Route("{id:int:min(1)}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();
            CompetitionDto competitionDto = new CompetitionDto(Competition);
           
            return Ok(competitionDto);
        }

        // GET /api/Competiion/FromPlayerAccount/1?ItemsOnPage=10&PageNumber=0
        [Authorize(Roles = "Administrator,Player,Moderator")]
        [Route("FromPlayerAccount/{id:int:min(1)}")]
        public async Task<IHttpActionResult> GetFromPlayerAccount(int id, [FromUri]PageBindingModel pageBindingModel)
        {
            PlayerAccountRepository playerAccountRepository = new PlayerAccountRepository(ContextProvider);

            PlayerAccount playerAccount = await playerAccountRepository.GetById(id);
            if (playerAccount == null)
                return NotFound();

            if (CanPlayerAccess(playerAccount) == false)
                return Unauthorized();

            PageViewModel<Competition> pageViewModel =
                await CompetitionRepository.GetCompetitionsByPlayerAccount(playerAccount, pageBindingModel);
            PageViewModel<CompetitionDto> pageViewModelDto = new PageViewModel<CompetitionDto>
            {
                TotalCount = pageViewModel.TotalCount,
                Items = pageViewModel.Items.Select(c => new CompetitionDto(c)).ToArray()
            };

            return Ok(pageViewModelDto);
        }

        // PUT /api/Competition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public async Task<IHttpActionResult> Put(int id, CompetitionDto competitionDto)
        {
            competitionDto.Id = id;

            if (await InitCompetitionById(id) == false)
                return NotFound();

            if (await CanOrganizerAccessAndEdit() == false)
                return Unauthorized();

            competitionDto.CopyDataFromDto(Competition);
            await CompetitionRepository.UpdateAsync(Competition);
            return Ok();
        }

        // POST /api/Competition/
        [Authorize(Roles = "Administrator")]
        [Route]
        public async Task<IHttpActionResult> Post(CompetitionDto competitionDto)
        {
            Competition competition = new Competition();
            competitionDto.CopyDataFromDto(competition);
            competition = await CompetitionRepository.AddAsync(competition);
            competitionDto.Id = competition.Id;
            return Created(Url.Content("~/api/Competition/"+ competition.Id), competitionDto);
        }

        // POST /api/Competition/1/OrganizerLock/true
        [Authorize(Roles = "Administrator")]
        [Route("{id:int:min(1)}/OrganizerLock/{b:bool}")]
        public async Task<IHttpActionResult> PostOrganizerEditLock(int id, bool b)
        {
            if (await InitCompetitionById(id) == false)
                return NotFound();

            Competition.OrganizerEditLock = b;

            await CompetitionRepository.UpdateAsync(Competition);

            return Ok();
        }

        // POST /api/Competition/Seed
        [Authorize(Roles = "Administrator")]
        [Route("Seed")]
        public async Task<IHttpActionResult> PostSeed()
        {
            IntegrationTests integrationTests = new IntegrationTests();
            await integrationTests.LoadCsvs();
            return Ok();
        }
    }
}