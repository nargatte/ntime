using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    [RoutePrefix("api/ExtraPlayerInfos")]
    public class ExtraPlayerInfoController : ControllerCompetitionIdBase<ExtraPlayerInfoRepository, ExtraPlayerInfo, ExtraPlayerInfoDto>
    {
        protected override ExtraPlayerInfoRepository CreateRepository() =>
            new ExtraPlayerInfoRepository(ContextProvider, Competition);

        protected override ExtraPlayerInfoDto CreateDto(ExtraPlayerInfo entity) =>
            new ExtraPlayerInfoDto(entity);

        protected override string CreatedAdress =>
            "extraplayerinfos";

        // GET api/ExtraPlayerInfos/FromCompetition/1
        [Route("FromCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> GetFromCompetition(int id) => 
            base.GetFromCompetition(id);

        // GET api/ExtraPlayerInfos/1
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Get(int id) => 
            base.Get(id);

        // PUT api/ExtraPlayerInfos/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("{id:int:min(1)}")]
        public override Task<IHttpActionResult> Put(int id, ExtraPlayerInfoDto entityDto) => 
            base.Put(id, entityDto);

        // POST /api/ExtraPlayerInfos/IntoCompetition/1
        [Authorize(Roles = "Administrator,Organizer")]
        [Route("IntoCompetition/{id:int:min(1)}")]
        public override Task<IHttpActionResult> PostIntoCompetition(int id, ExtraPlayerInfoDto entityDto) => 
            base.PostIntoCompetition(id, entityDto);

        // DELETE api/ExtraPlayerInfos/1
        [Route("{id:int:min(1)}")]
        [Authorize(Roles = "Administrator")]
        public override Task<IHttpActionResult> Delete(int id) => 
            base.Delete(id);
    }
}