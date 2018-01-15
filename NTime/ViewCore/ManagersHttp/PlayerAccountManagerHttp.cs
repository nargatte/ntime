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
        private HttpPlayerAccountClient _client;
        private PlayerAccount _loggedInPlayer;

        public PlayerAccount LoggedInPlayer => _loggedInPlayer;

        public PlayerAccountManagerHttp(DependencyContainer dependencyContainer) : base(dependencyContainer.User, dependencyContainer.ConnectionInfo)
        {
            _client = new HttpPlayerAccountClient(dependencyContainer.User, dependencyContainer.ConnectionInfo, "PlayerAccount");
        }

        public async Task<PlayerAccount> DownloadPlayerTemplateData()
        {
            await TryCallApi(async () =>
            {
                _loggedInPlayer = await GetLoggedInPlayerAccount();
            });
            return LoggedInPlayer;
        }

        public async Task UpdatePlayerTemplateData(PlayerAccount playerAccount)
        {
            await TryCallApi(async () =>
            {
                _loggedInPlayer = playerAccount;
                await _client.UpdatePlayerAccountInfo(playerAccount);
            });
            if (IsSuccess)
            {
                MessageBox.Show("Dane został poprawnie zapisane");
            }
        }

        public async Task<PlayerAccount> GetLoggedInPlayerAccount()
        {
            PlayerAccount loggedInPlayerAccount = new PlayerAccount();
            await TryCallApi(async () =>
            {
                loggedInPlayerAccount = (await _client.GetLoggedInPlayerAccount()).CopyDataFromDto(new PlayerAccount());
            });
            return loggedInPlayerAccount;
        }
    }
}
