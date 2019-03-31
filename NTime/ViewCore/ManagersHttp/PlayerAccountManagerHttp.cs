using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using ViewCore.Factories;
using ViewCore.HttpClients;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersHttp
{
    public class PlayerAccountManagerHttp : ManagerHttp, IPlayerAccountManager
    {
        private readonly HttpPlayerAccountClient _client;

        public PlayerAccount LoggedInPlayer { get; private set; }

        public PlayerAccountManagerHttp(DependencyContainer dependencyContainer) : base(dependencyContainer.User, dependencyContainer.ConnectionInfo)
        {
            _client = new HttpPlayerAccountClient(dependencyContainer.User, dependencyContainer.ConnectionInfo, "PlayerAccount");
        }

        public async Task<PlayerAccount> DownloadPlayerTemplateData()
        {
            await TryCallApi(async () =>
            {
                LoggedInPlayer = await GetLoggedInPlayerAccount();
            });
            return LoggedInPlayer;
        }

        public Task UpdatePlayerTemplateData(PlayerAccount playerAccount)
        {
            //await TryCallApi(async () =>
            //{
            //    _loggedInPlayer = playerAccount;
            //    await _client.UpdatePlayerAccountInfo(playerAccount);
            //});
            //if (IsSuccess)
            //{
            //    MessageBox.Show("Dane został poprawnie zapisane");
            //}
            throw new NotImplementedException();
        }

        public Task<PlayerAccount> GetLoggedInPlayerAccount()
        {
            throw new NotImplementedException();
        }
    }
}
