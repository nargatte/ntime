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
    public class AgeCategoryManagerHttp : ManagerHttp, IAgeCategoryManager
    {
        IEditableCompetition _currentCompetition;
        HttpAgeCategoryClient _client;

        public ObservableCollection<EditableAgeCategory> DefinedAgeCategories { get; set; } = new ObservableCollection<EditableAgeCategory>();

        public AgeCategoryManagerHttp(AccountInfo accountInfo, ConnectionInfo connectionInfo, IEditableCompetition currentCompetition) : base(accountInfo, connectionInfo)
        {
            _currentCompetition = currentCompetition;
            _client = new HttpAgeCategoryClient(accountInfo, connectionInfo, "AgeCategory");
        }

        public async Task<ObservableCollection<EditableAgeCategory>> DownloadAgeCategoriesAsync()
        {
            await TryCallApi(async () =>
            {
                var dtoAgeCategories = await _client.GetAllFromCompetitionAsync(_currentCompetition.DbEntity.Id);
                DefinedAgeCategories.Clear();
                foreach (var dtoAgeCategory in dtoAgeCategories)
                {
                    DefinedAgeCategories.Add(new EditableAgeCategory(_currentCompetition)
                    {
                        DbEntity = dtoAgeCategory.CopyDataFromDto(new AgeCategory())
                    });
                }
                DefinedAgeCategories.Add(new EditableAgeCategory(_currentCompetition)
                {
                    DbEntity = new AgeCategory()
                });
            });
            return DefinedAgeCategories;
        }
    }
}
