using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;
using ViewCore.Entities;

namespace ViewCore.HttpClients
{
    public abstract class HttpForSpecificCompetitionClient<TEntity, TDto> : HttpClientBase
        where TDto : IDtoBase<TEntity>
    {
        //EditableCompetition _editableCompetition;

        protected HttpForSpecificCompetitionClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName) 
            : base(accountInfo, connectionInfo, controllerName)
        {
            //_editableCompetition = editableCompetition;
        }

        protected abstract TDto CreateDto(TEntity entity);

        public async Task<TDto> GetAsync(int id)
        {
            return await base.GetAsync<TDto>($"/{id.ToString()}");
        }
        
        public async Task<TDto> GetAllFromCompetitionAsync(int competitionId)
        {
            return await base.GetAsync<TDto>($"/FromCompetition/{competitionId}");
        }

        public async Task UpdateAsync(TEntity content)
        {
            var contentDto = CreateDto(content);
            await base.PutAsync<TDto>($"/{contentDto.Id.ToString()}", contentDto);
        }

        //TODO It is possible that I cannot use the TResult in here
        /// <summary>
        /// Returned object will be different
        /// </summary>
        /// <param name="competitionId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual async Task<TDto> AddToCompetitionAsync(int competitionId, TEntity content)
        {
            var contentDto = CreateDto(content);
            return await base.PostAsync<TDto, TDto>($"/IntoCompetition/{competitionId}", contentDto);
        }

        public virtual async Task<ReturnedType> AddToCompetitionAsync<ReturnedType>(int competitionId, TEntity content)
        {
            var contentDto = CreateDto(content);
            return await base.PostAsync<TDto, ReturnedType>($"/IntoCompetition/{competitionId}", contentDto);
        }

        public async Task DeleteAsync(TEntity content)
        {
            var contentDto = CreateDto(content);
            await base.DeleteAsync($"/{contentDto.Id.ToString()}");
        }
    }
}
