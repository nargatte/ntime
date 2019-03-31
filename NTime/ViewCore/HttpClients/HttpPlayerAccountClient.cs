using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;

namespace ViewCore.HttpClients
{
    public class HttpPlayerAccountClient : HttpClientBase
    {
        public HttpPlayerAccountClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        public async Task<PlayerAccountDto> GetPlayerAccountAsync(int id)
        {
            return await base.GetAsync<PlayerAccountDto>($"{id}");
        }
        
        public async Task<PlayerAccountDto> GetLoggedInPlayerAccount()
        {
            return await base.GetAsync<PlayerAccountDto>(null);
        }

        public Task UpdatePlayerAccountInfo(PlayerAccount playerAccount)
        {
            throw new NotImplementedException();
        }
    }
}
