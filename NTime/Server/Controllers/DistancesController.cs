using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/Distances")]
    public class DistancesController : ApiController
    {
        ContextProvider ContextProvider = new ContextProvider();

        // GET api/Distances/FromCompetition/1
        [Route("FromCompetition/{id}")]
        public async Task<IHttpActionResult> GetFromCompetition(int id)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetById(id);

            if (competition == null)
            {
                return NotFound();
            }

            DistanceRepository distanceRepository = new DistanceRepository(ContextProvider, competition);
            Distance[] distances = await distanceRepository.GetAllAsync();
            DistanceDto[] distanceDtos = distances.Select(d => new DistanceDto(d)).ToArray();

            return Ok(distanceDtos);
        }

        // GET api/Distances/1
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetByRelatedEntitieId<Distance>(id);

            if (competition == null)
            {
                return NotFound();
            }

            DistanceRepository distanceRepository = new DistanceRepository(ContextProvider, competition);
            Distance distance = await distanceRepository.GetById(id);
            DistanceDto distanceDto = new DistanceDto(distance);

            return Ok(distanceDto);
        }

        // PUT api/Distances/1
        [Authorize(Roles = "Administrator,Moderator")]
        [Route("{id}")]
        public async Task<IHttpActionResult> Put(int id, DistanceDto distanceDto)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetByRelatedEntitieId<Distance>(id);

            if (competition == null)
            {
                return NotFound();
            }

            if (!await ControllerHelper.ModeratorAcess(User, ContextProvider, competition.Id))
                return Unauthorized();

            DistanceRepository distanceRepository = new DistanceRepository(ContextProvider, competition);
            Distance distance = await distanceRepository.GetById(id);

            distanceDto.CopyDataFromDto(distance);
            await distanceRepository.UpdateAsync(distance);
            return Ok();
        }

        // POST /api/Distances/forcompetition/1
        [Authorize(Roles = "Administrator,Moderator")]
        [Route("ForCompetition/{id}")]
        public async Task<IHttpActionResult> PostForCompetition(int id, DistanceDto distanceDto)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetById(id);

            if (competition == null)
            {
                return NotFound();
            }

            if (!await ControllerHelper.ModeratorAcess(User, ContextProvider, id))
                return Unauthorized();

            DistanceRepository distanceRepository = new DistanceRepository(ContextProvider, competition);
            Distance distance = new Distance();

            distanceDto.CopyDataFromDto(distance);
            distance = await distanceRepository.AddAsync(distance);
            return Created(Url.Content("~/api/Distances/" + distance.Id), distanceDto);
        }

        //DELETE api/Distances/1
        [Authorize(Roles = "Administrator,Moderator")]
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            CompetitionRepository competitionRepository = new CompetitionRepository(ContextProvider);
            Competition competition = await competitionRepository.GetByRelatedEntitieId<Distance>(id);

            if (competition == null)
            {
                return NotFound();
            }

            if (!await ControllerHelper.ModeratorAcess(User, ContextProvider, competition.Id))
                return Unauthorized();

            DistanceRepository distanceRepository = new DistanceRepository(ContextProvider, competition);
            Distance distance = await distanceRepository.GetById(id);
            await distanceRepository.RemoveAsync(distance);
            return Ok();
        }
    }
}