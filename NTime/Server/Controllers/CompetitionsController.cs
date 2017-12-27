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
    public class CompetitionsController : ControllerNTimeBase
    {
        protected CompetitionsController() : base()
        {
        }

        // GET /api/competitions?ItemsOnPage=10&PageNumber=0
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

        // GET /api/competitions/1
        public async Task<IHttpActionResult> Get(int id)
        {
            if (await InitComprtitionById(id) == false)
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

        // PUT /api/competitions/1
        [Authorize(Roles = "Administrator,Organizer")]
        public async Task<IHttpActionResult> Put(int id, CompetitionDto competitionDto)
        {
            if (await InitComprtitionById(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            competitionDto.CopyDataFromDto(Competition);
            await CompetitionRepository.UpdateAsync(Competition);
            return Ok();
        }

        // POST /api/competitions/
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(CompetitionDto competitionDto)
        {
            Competition competition = new Competition();
            competitionDto.CopyDataFromDto(competition);
            competition = await CompetitionRepository.AddAsync(competition);
            competitionDto.Id = competition.Id;
            return Created(Url.Content("~/api/competitions/"+ competition.Id), competitionDto);
        }
    }
}