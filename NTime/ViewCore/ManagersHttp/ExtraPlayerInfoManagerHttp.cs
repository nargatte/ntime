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
    public class ExtraPlayerInfoManagerHttp : ManagerHttp, IExtraPlayerInfoManager
    {
        private IEditableCompetition _currentCompetition;
        private HttpExtraPlayerInfoClient _client;
        public ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo { get; set; } = new ObservableCollection<EditableExtraPlayerInfo>();

        public ExtraPlayerInfoManagerHttp(IEditableCompetition currentCompetition, AccountInfo accountInfo, ConnectionInfo connectionInfo) : base(accountInfo, connectionInfo)
        {
            _currentCompetition = currentCompetition;
            _client = new HttpExtraPlayerInfoClient(accountInfo, connectionInfo, "Subcategories");
        }

        public async Task<ObservableCollection<EditableExtraPlayerInfo>> DownloadExtraPlayerInfoAsync()
        {
            await TryCallApi(async () =>
            {
                var dtoExtraPlayerInfos = await _client.GetAllFromCompetitionAsync(_currentCompetition.DbEntity.Id);
                DefinedExtraPlayerInfo.Clear();
                foreach (var dtoExtraPlayerInfo in dtoExtraPlayerInfos)
                {
                    DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
                    {
                        DbEntity = dtoExtraPlayerInfo.CopyDataFromDto(new Subcategory())
                    });
                }
                DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
                {
                    DbEntity = new Subcategory()
                });
            });
            return DefinedExtraPlayerInfo;
        }
    }
}
