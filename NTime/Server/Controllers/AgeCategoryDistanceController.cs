using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/AgeCategoryDistance")]
    public class AgeCategoryDistanceController : ControllerCompetitionIdBase<AgeCategoryDistanceRepository,
        AgeCategoryDistance, AgeCategoryDistanceDto>
    {

        protected override AgeCategoryDistanceRepository CreateRepository() =>
            new AgeCategoryDistanceRepository(ContextProvider, Competition);

        protected override AgeCategoryDistanceDto CreateDto(AgeCategoryDistance entity) =>
            new AgeCategoryDistanceDto(entity);

        protected override string CreatedAdress =>
            "AgeCategoryDistance";

        // GET api/AgeCategoryDistance/FromCompetition/1
        [Route("FromCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) =>
            base.GetFromCompetition(id);

        // GET api/AgeCategoryDistance/1
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Get(int id) =>
            base.Get(id);

        // PUT api/AgeCategoryDistance/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Put(int id, AgeCategoryDistanceDto AgeCategoryDistanceDto) =>
            base.Put(id, AgeCategoryDistanceDto);

        // POST /api/AgeCategoryDistance/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id,
            AgeCategoryDistanceDto AgeCategoryDistanceDto) =>
            base.PostIntoCompetition(id, AgeCategoryDistanceDto);

        // DELETE api/AgeCategoryDistance/1
        [Authorize(Roles = "Administrator")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Delete(int id) =>
            base.Delete(id);

        public AgeCategoryDistanceController() : base()
        {
        }
    }
}