using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.HttpClients;

namespace ViewCore.ManagersHttp
{
    public class AuthenticationManagerHttp : ManagerHttp
    {
        private HttpAuthenticationClient _client;

        public AuthenticationManagerHttp(AccountInfo accountInfo, ConnectionInfo connectionInfo) : base(accountInfo, connectionInfo)
        {
            _client = new HttpAuthenticationClient(accountInfo, connectionInfo, string.Empty);
        }

        public async Task<bool> Register(string email, string password, string confirmPassword)
        {
            await TryCallApi(async() =>
            {
                await _client.RegisterUser(new Server.Models.RegisterBindingModel()
                {
                    Email = email,
                    Password = password,
                    ConfirmPassword = confirmPassword
                });
            });
            return IsSuccess;
        }

        public async Task<bool> Login(string email, string password)
        {
            await TryCallApi(async() =>
            {
                var tempAccountInfo = await _client.Login(email, password);
                _accountInfo.Token = tempAccountInfo.access_token;
                _accountInfo.UserName = tempAccountInfo.userName;
            });
            if (IsSuccess)
            {
                _client.SetAuthenticationData(_accountInfo);
            }
            return IsSuccess;
        }

        public async Task<bool> Logout()
        {
            await TryCallApi(async () =>
            {
                await _client.Logout();
            });
            if (IsSuccess)
            {
                _accountInfo.Token = "";
                _accountInfo.UserName = "";
                _client.SetAuthenticationData(_accountInfo);
            }
            return IsSuccess;
        }
    }
}
