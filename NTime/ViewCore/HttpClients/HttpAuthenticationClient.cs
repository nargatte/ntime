using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Server.Dtos;
using Server.Models;

namespace ViewCore.HttpClients
{
    public class HttpAuthenticationClient : HttpClientBase
    {
        public HttpAuthenticationClient(AccountInfo accountInfo, ConnectionInfo connectionInfo, string controllerName)
            : base(accountInfo, connectionInfo, controllerName)
        {
        }

        public async Task RegisterUser(RegisterBindingModel registerModel)
        {
            await PostAsync("/api/Account/Register", registerModel);
        }

        public async Task<TokenInfoDto> Login(string email, string password)
        {
            return await PostAsync<LoginDataDto, TokenInfoDto>("/token", new LoginDataDto(email, password));
        }
    }
}
