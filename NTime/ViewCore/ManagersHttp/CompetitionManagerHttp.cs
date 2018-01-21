using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.HttpClients;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersHttp
{
    class CompetitionManagerHttp : ManagerHttp, ICompetitionManager
    {
        private HttpCompetitionClient _client;
        private DependencyContainer _dependencyContainer;
        private ObservableCollection<EditableCompetition> _competitions = new ObservableCollection<EditableCompetition>();

        public CompetitionManagerHttp(DependencyContainer dependencyContainer) : base(dependencyContainer.User, dependencyContainer.ConnectionInfo)
        {
            _client = new HttpCompetitionClient(dependencyContainer.User, dependencyContainer.ConnectionInfo, "Competition");
            _dependencyContainer = dependencyContainer;
        }

        public ObservableCollection<EditableCompetition> GetCompetitionsToDisplay() => _competitions;

        public async Task AddAsync(Competition dbEntity)
        {
            await TryCallApi(async () =>
            {
                await _client.AddAsync(dbEntity);
            });
        }

        public void ClearDatabase()
        {
            System.Windows.MessageBox.Show("Nie masz uprawnień, by usunąć wszystkie zawody");
        }

        public async Task DownloadDataFromDatabase()
        {
            await TryCallApi(async () =>
            {
                var dtoCompetitions = await _client.GetPageAsync(100, 0);
                _competitions.Clear();
                foreach (var dbCompetition in dtoCompetitions.Items.Select(dto => dto.CopyDataFromDto(new Competition())))
                {
                    _competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
                }
            });
        }

        public async Task GetCompetitionsForPlayerAccount()
        {
            await TryCallApi(async () =>
            {
                var playerAccountManager = new PlayerAccountManagerHttp(_dependencyContainer);
                var loggedInAccount = await playerAccountManager.GetLoggedInPlayerAccount();
                //await _client.GetPageFromPlayerAccountAsync(loggedInAccount, 100, 0);
                var dtoCompetitions = await _client.GetPageFromPlayerAccountAsync(loggedInAccount, 100, 0);
                _competitions.Clear();
                foreach (var dbCompetition in dtoCompetitions.Items.Select(dto => dto.CopyDataFromDto(new Competition())))
                {
                    _competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
                }
            });
        }
    }
}
