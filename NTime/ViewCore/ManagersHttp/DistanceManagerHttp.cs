using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using ViewCore.Entities;
using ViewCore.HttpClients;
using ViewCore.ManagersInterfaces;

namespace ViewCore.ManagersHttp
{
    public class DistanceManagerHttp : ManagerHttp, IDistanceManager
    {
        IEditableCompetition _currentCompetition;
        HttpDistanceClient _client;
        public ObservableCollection<EditableDistance> DefinedDistances { get; set; } = new ObservableCollection<EditableDistance>();

        public DistanceManagerHttp(AccountInfo accountInfo, ConnectionInfo connectionInfo, IEditableCompetition currentCompetition) : base(accountInfo, connectionInfo)
        {
            _currentCompetition = currentCompetition;
            _client = new HttpDistanceClient(accountInfo, connectionInfo, "Distance");
        }

        public async Task<ObservableCollection<EditableDistance>> DownloadDistancesAsync()
        {
            await TryCallApi(async () =>
            {
                var dbtoDistances = await _client.GetAllFromCompetitionAsync(_currentCompetition.DbEntity.Id);
                DefinedDistances.Clear();
                foreach (var dtoDistance in dbtoDistances)
                {
                    DefinedDistances.Add(new EditableDistance(_currentCompetition)
                    {
                        DbEntity = dtoDistance.CopyDataFromDto(new Distance())
                    });
                }
                DefinedDistances.Add(new EditableDistance(_currentCompetition)
                {
                    DbEntity = new Distance()
                });
            });
            return DefinedDistances;
        }
    }
}
