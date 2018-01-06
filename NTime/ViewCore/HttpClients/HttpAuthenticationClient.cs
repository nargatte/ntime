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
            await base.PostAsync("api/Account/Register", registerModel);
        }

        public async Task<TokenInfoDto> Login(string email, string password)
        {
            var loginData = new LoginDataDto(email, password);
            var content = loginData.GetDictionary();
            return await base.PostUrlEncodedAsync<TokenInfoDto>("token", content);
        }

        public async Task Logout()
        {
            await base.PostAsync("api/Account/Logout");
        }
    }
}
