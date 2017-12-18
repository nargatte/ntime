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
    public class CompetitionsController : ApiController
    {
        private readonly ContextProvider _contextProvider = new ContextProvider();
        private readonly CompetitionRepository _competitionRepository;

        public CompetitionsController()
        {
            _competitionRepository =  new CompetitionRepository(_contextProvider);
        }

        // GET /api/competitions?ItemsOnPage=10&PageNumber=0
        public async Task<PageViewModel<CompetitionDto>> Get([FromUri]PageBindingModel pageBindingModel)
        {
            PageViewModel<Competition> pageViewModel = await _competitionRepository.GetAllAsync(pageBindingModel);
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
            Competition competition = await _competitionRepository.GetById(id);
            if (competition == null)
            {
                return NotFound();
            }
            CompetitionDto competitionDto = new CompetitionDto(competition);
            DistanceRepository distanceRepository = new DistanceRepository(_contextProvider, competition);
            ExtraPlayerInfoRepository extraPlayerInfoRepository = new ExtraPlayerInfoRepository(_contextProvider, competition);
            competitionDto.Distances = (await distanceRepository.GetAllAsync())
                .Select(d => new NameIdModel(d.Id, d.Name)).ToArray();
            competitionDto.ExtraPlayerInfo = (await extraPlayerInfoRepository.GetAllAsync())
                .Select(d => new NameIdModel(d.Id, d.Name)).ToArray();
            return Ok(competitionDto);
        }

        // PUT /api/competitions/
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IHttpActionResult> Put(CompetitionDto competitionDto)
        {
            Competition competition = await _competitionRepository.GetById(competitionDto.Id);
            var user = User.Identity;
            ApplicationDbContext context = new ApplicationDbContext();
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var s = UserManager.GetRoles(user.GetUserId());
            if (competition == null)
            {
                return NotFound();
            }
            if (s[0] == "Moderator" && !await _competitionRepository.CanModeratorEdit(user.GetUserId()))
                return Unauthorized();
            competitionDto.CopyDataFromDto(competition);
            await _competitionRepository.UpdateAsync(competition);
            return Ok();
        }

        // POST /api/competitions/
        [Authorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> Post(CompetitionDto competitionDto)
        {
            Competition competition = new Competition();
            competitionDto.CopyDataFromDto(competition);
            competition = await _competitionRepository.AddAsync(competition);
            return Created(Url.Content("~/api/competitions/"+ competition.Id), competition);
        }
    }
}