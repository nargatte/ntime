using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BaseCore.DataBase;
using Server.Dtos;

namespace Server.Controllers
{
    public abstract class ControllerCompetitionIdBase<Tr, Te, Td> : ControllerNTimeBase
        where Tr: RepositoryCompetitionId<Te>
        where Te: class, ICompetitionId, IEntityId, new()
        where Td: class, IDtoBase<Te>
    {
        private Tr Repository;

        protected abstract Tr CreateRepository();
        protected abstract Td CreateDto(Te entity);
        protected abstract string CreatedAdress { get; }

        protected ControllerCompetitionIdBase() : base()
        {
        }

        protected override async Task<bool> InitCompetytionByRelatedEntitieId<T>(int id)
        {
            if (await base.InitCompetytionByRelatedEntitieId<T>(id) == false)
                return false;
            Repository = CreateRepository();
            return true;
        }

        protected override async Task<bool> InitComprtitionById(int Id)
        {
            if (await base.InitComprtitionById(Id) == false)
                return false;
            Repository = CreateRepository();
            return true;
        }

        public virtual async Task<IHttpActionResult> GetFromCompetition(int id)
        {
            if (await InitComprtitionById(id) == false)
            {
                return NotFound();
            }

            Te[] enties = await Repository.GetAllAsync();
            Td[] entitiesDtos = enties.Select(CreateDto).ToArray();

            return Ok(entitiesDtos);
        }

        public virtual async Task<IHttpActionResult> Get(int id)
        {
            if (await InitCompetytionByRelatedEntitieId<Te>(id) == false)
                return NotFound();

            Te entity = await Repository.GetById(id);
            Td entityDto = CreateDto(entity);

            return Ok(entityDto);
        }

        public virtual async Task<IHttpActionResult> Put(int id, Td entityDto)
        {
            if (await InitCompetytionByRelatedEntitieId<Te>(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Te entity = await Repository.GetById(id);

            entityDto.CopyDataFromDto(entity);
            await Repository.UpdateAsync(entity);

            return Ok();
        }

        public virtual async Task<IHttpActionResult> PostIntoCompetition(int id, Td entityDto)
        {
            if (await InitComprtitionById(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Te entity = new Te();

            entityDto.CopyDataFromDto(entity);
            entity = await Repository.AddAsync(entity);
            entityDto.Id = entity.Id;

            return Created(Url.Content("~/api/"+ CreatedAdress +"/" + entity.Id), entityDto);
        }

        public virtual async Task<IHttpActionResult> Delete(int id)
        {
            if (await InitCompetytionByRelatedEntitieId<Te>(id) == false)
                return NotFound();

            if (await CanOrganizerAccess() == false)
                return Unauthorized();

            Te entity = await Repository.GetById(id);
            await Repository.RemoveAsync(entity);
            return Ok();
        }
    }
}