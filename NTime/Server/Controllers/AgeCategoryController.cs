using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/AgeCategory")]
    public class AgeCategoryController : ControllerCompetitionIdBase<AgeCategoryRepository, AgeCategory, AgeCategoryDto>
    {

        protected override AgeCategoryRepository CreateRepository() =>
            new AgeCategoryRepository(ContextProvider, Competition);

        protected override AgeCategoryDto CreateDto(AgeCategory entity) =>
            new AgeCategoryDto(entity);

        protected override string CreatedAdress =>
            "AgeCategory";

        // GET api/AgeCategory/FromCompetition/1
        [Route("FromCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) =>
            base.GetFromCompetition(id);

        // GET api/AgeCategory/1
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Get(int id) =>
            base.Get(id);

        // PUT api/AgeCategory/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Put(int id, AgeCategoryDto ageCategoryDto) =>
            base.Put(id, ageCategoryDto);

        // POST /api/AgeCategory/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id, AgeCategoryDto ageCategoryDto) =>
            base.PostIntoCompetition(id, ageCategoryDto);

        // DELETE api/AgeCategory/1
        [Authorize(Roles = "Administrator")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Delete(int id) =>
            base.Delete(id);

        public AgeCategoryController() : base()
        {
        }
    }
}