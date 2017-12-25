using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/Distances")]
    public class DistancesController : ControllerCompetitionIdBase<DistanceRepository, Distance, DistanceDto>
    {
        protected override DistanceRepository CreateRepository() =>
            new DistanceRepository(ContextProvider, Competition);

        protected override DistanceDto CreateDto(Distance entity) =>
            new DistanceDto(entity);

        protected override string CreatedAdress =>
            "distances";

        // GET api/Distances/FromCompetition/1
        [Route("FromCompetition/{id}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) =>
            base.GetFromCompetition(id);

        // GET api/Distances/1
        [Route("{id}")]
        public override Task<IHttpActionResult> Get(int id) =>
            base.Get(id);

        // PUT api/Distances/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id}")]
        public override Task<IHttpActionResult> Put(int id, DistanceDto distanceDto) =>
            base.Put(id, distanceDto);

        // POST /api/Distances/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id, DistanceDto distanceDto) =>
            base.PostIntoCompetition(id, distanceDto);

        // DELETE api/Distances/1
        [Authorize(Roles = "Administrator")]
        [Route("{id}")]
        public override Task<IHttpActionResult> Delete(int id) =>
            base.Delete(id);

        public DistancesController() : base()
        {
        }
    }
}