using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/AgeCategories")]
    public class AgeCategoriesController : ControllerCompetitionIdBase<AgeCategoryRepository, AgeCategory, AgeCategoryDto>
    {

        protected override AgeCategoryRepository CreateRepository() =>
            new AgeCategoryRepository(ContextProvider, Competition);

        protected override AgeCategoryDto CreateDto(AgeCategory entity) =>
            new AgeCategoryDto(entity);

        protected override string CreatedAdress =>
            "agecategories";

        // GET api/AgeCategories/FromCompetition/1
        [Route("FromCompetition/{id}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) =>
            base.GetFromCompetition(id);

        // GET api/AgeCategories/1
        [Route("{id}")]
        public override Task<IHttpActionResult> Get(int id) =>
            base.Get(id);

        // PUT api/AgeCategories/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id}")]
        public override Task<IHttpActionResult> Put(int id, AgeCategoryDto ageCategoryDto) =>
            base.Put(id, ageCategoryDto);

        // POST /api/AgeCategories/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id, AgeCategoryDto ageCategoryDto) =>
            base.PostIntoCompetition(id, ageCategoryDto);

        // DELETE api/AgeCategories/1
        [Authorize(Roles = "Administrator")]
        [Route("{id}")]
        public override Task<IHttpActionResult> Delete(int id) =>
            base.Delete(id);

        public AgeCategoriesController() : base()
        {
        }
    }
}