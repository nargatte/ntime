using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/Subcategory")]
    public class SubcategoryController : ControllerCompetitionIdBase<SubcategoryRepository, Subcategory, SubcategoryDto>
    {
        protected override SubcategoryRepository CreateRepository() =>
            new SubcategoryRepository(ContextProvider, Competition);

        protected override SubcategoryDto CreateDto(Subcategory entity) =>
            new SubcategoryDto(entity);

        protected override string CreatedAdress =>
            "subcategory";

        // GET api/Subcategories/FromCompetition/1
        [Route("FromCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) => 
            base.GetFromCompetition(id);

        // GET api/Subcategories/1
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Get(int id) => 
            base.Get(id);

        // PUT api/Subcategories/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Put(int id, SubcategoryDto entityDto) => 
            base.Put(id, entityDto);

        // POST /api/Subcategories/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id, SubcategoryDto entityDto) => 
            base.PostIntoCompetition(id, entityDto);

        // DELETE api/Subcategories/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public override Task<IHttpActionResult> Delete(int id) => 
            base.Delete(id);
    }
}