using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.Dtos;

namespace ViewCore.HttpClients
{
    public class HttpCompetitionClient : HttpClientBase
    {
        public HttpCompetitionClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        public async Task<CompetitionDto> Get(int id)
        {
            return await base.GetAsync<CompetitionDto>(id.ToString());
        }

        public async Task<CompetitionDto> Update(CompetitionDto competition)
        {
            return await base.PutAsync<CompetitionDto, CompetitionDto>(competition.Id.ToString(), competition);
        }

        public async Task Delete(CompetitionDto competition)
        {
            await base.DeleteAsync(competition.Id.ToString());
            return;
        }
    }
}
