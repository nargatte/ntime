using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using Server.Dtos;
using ViewCore.Entities;
using ViewCore.HttpClients;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersHttp
{
    class CompetitionManagerHttp : ManagerHttp, ICompetitionManager
    {
        private HttpCompetitionClient _client;
        private ObservableCollection<EditableCompetition> _competitions = new ObservableCollection<EditableCompetition>();

        public CompetitionManagerHttp(AccountInfo accountInfo, ConnectionInfo connectionInfo) : base(accountInfo, connectionInfo)
        {
            _client = new HttpCompetitionClient(accountInfo, connectionInfo,"Competition");
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

        public async void DownloadDataFromDatabase()
        {
            var dtoCompetitions = await _client.GetPageAsync(100, 0);
            _competitions.Clear();
            foreach (var dbCompetition in dtoCompetitions.Items.Select(dto => dto.CopyDataFromDto(new Competition())))
            {
                _competitions.Add(new EditableCompetition() { DbEntity = dbCompetition });
            }
        }
    }
}
