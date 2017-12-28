using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using BaseCore.Dtos;
using BaseCore.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Server.Models;

namespace Server.Controllers
{
    public class CompetitionController : ControllerNTimeBase
    {
        protected CompetitionController() : base()
        {
        }

        // GET /api/Competition?ItemsOnPage=10&PageNumber=0
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
            {
                return NotFound();
            }
            CompetitionDto competitionDto = new CompetitionDto(Competition);
            DistanceRepository distanceRepository = new DistanceRepository(ContextProvider, Competition);
            ExtraPlayerInfoRepository extraPlayerInfoRepository = new ExtraPlayerInfoRepository(ContextProvider, Competition);
            competitionDto.Distances = (await distanceRepository.GetAllAsync())
                .Select(d => new NameIdModel(d.Id, d.Name)).ToArray();
            competitionDto.ExtraPlayerInfo = (await extraPlayerInfoRepository.GetAllAsync())
                .Select(d => new NameIdModel(d.Id, d.Name)).ToArray();
            return Ok(competitionDto);
        }

        // PUT /api/Competition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public async Task<IHttpActionResult> Put(int id, CompetitionDto competitionDto)
        {
            competitionDto.Id = id;

            if (await InitCompetitionById(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            competitionDto.CopyDataFromDto(Competition);
            await CompetitionRepository.UpdateAsync(Competition);
            return Ok();
        }

        // POST /api/Competition/
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(CompetitionDto competitionDto)
        {
            Competition competition = new Competition();
            competitionDto.CopyDataFromDto(competition);
            competition = await CompetitionRepository.AddAsync(competition);
            competitionDto.Id = competition.Id;
            return Created(Url.Content("~/api/Competition/"+ competition.Id), competitionDto);
        }
    }
}