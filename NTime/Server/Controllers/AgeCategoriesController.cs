using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/AgeCategories")]
    public class AgeCategoriesController : ApiController
    {
        ContextProvider ContextProvider = new ContextProvider();

        // GET api/agecategories/FromCompetition/1
        [Route("FromCompetition/{id}")]
        public async Task<IHttpActionResult> GetFromCompetition(int id)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetById(id);

            if (competition == null)
            {
                return NotFound();
            }

            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(ContextProvider, competition);
            AgeCategory[] ageCategories = await ageCategoryRepository.GetAllAsync();
            AgeCategoryDto[] ageCategoryDtos = ageCategories.Select(ac => new AgeCategoryDto(ac)).ToArray();

            return Ok(ageCategoryDtos);
        }

        // GET api/agecategories/1
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetByRelatedEntitieId<AgeCategory>(id);

            if (competition == null)
            {
                return NotFound();
            }

            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(ContextProvider, competition);
            AgeCategory ageCategory = await ageCategoryRepository.GetById(id);
            AgeCategoryDto ageCategoryDto = new AgeCategoryDto(ageCategory);

            return Ok(ageCategoryDto);
        }

        // PUT api/agecategories/1
        [Authorize(Roles = "Administrator,Moderator")]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, AgeCategoryDto ageCategoryDto)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetByRelatedEntitieId<AgeCategory>(id);

            if (competition == null)
            {
                return NotFound();
            }

            if (!await ControllerHelper.ModeratorAcess(User, ContextProvider, competition.Id))
                return Unauthorized();

            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(ContextProvider, competition);
            AgeCategory ageCategory = await ageCategoryRepository.GetById(id);

            ageCategoryDto.CopyDataFromDto(ageCategory);
            await ageCategoryRepository.UpdateAsync(ageCategory);
            return Ok();
        }

        // POST /api/agecategories/forcompetition/1
        [Authorize(Roles = "Administrator,Moderator")]
        [Route("ForCompetition/{id}")]
        public async Task<IHttpActionResult> PostForCompetition(int id, AgeCategoryDto ageCategoryDto)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetById(id);

            if (competition == null)
            {
                return NotFound();
            }

            if (!await ControllerHelper.ModeratorAcess(User, ContextProvider, id))
                return Unauthorized();

            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(ContextProvider, competition);
            AgeCategory ageCategory = new AgeCategory();

            ageCategoryDto.CopyDataFromDto(ageCategory);
            ageCategory = await ageCategoryRepository.AddAsync(ageCategory);
            return Created(Url.Content("~/api/agecategories/" + ageCategory.Id), ageCategoryDto);
        }

        //DELETE api/agecategories/1
        [Authorize(Roles = "Administrator,Moderator")]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetByRelatedEntitieId<AgeCategory>(id);

            if (competition == null)
            {
                return NotFound();
            }

            if (!await ControllerHelper.ModeratorAcess(User, ContextProvider, competition.Id))
                return Unauthorized();

            AgeCategoryRepository ageCategoryRepository = new AgeCategoryRepository(ContextProvider, competition);
            AgeCategory ageCategory = await ageCategoryRepository.GetById(id);
            await ageCategoryRepository.RemoveAsync(ageCategory);
            return Ok();
        }
    }
}