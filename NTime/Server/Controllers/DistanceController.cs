using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/Distance")]
    public class DistanceController : ControllerCompetitionIdBase<DistanceRepository, Distance, DistanceDto>
    {
        protected override DistanceRepository CreateRepository() =>
            new DistanceRepository(ContextProvider, Competition);

        protected override DistanceDto CreateDto(Distance entity) =>
            new DistanceDto(entity);

        protected override string CreatedAdress =>
            "Distance";

        // GET api/Distance/FromCompetition/1
        [Route("FromCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) =>
            base.GetFromCompetition(id);

        // GET api/Distance/1
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Get(int id) =>
            base.Get(id);

        // PUT api/Distance/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Put(int id, DistanceDto distanceDto) =>
            base.Put(id, distanceDto);

        // POST /api/Distance/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id, DistanceDto distanceDto) =>
            base.PostIntoCompetition(id, distanceDto);

        // DELETE api/Distance/1
        [Authorize(Roles = "Administrator")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Delete(int id) =>
            base.Delete(id);

        public DistanceController() : base()
        {
        }
    }
}