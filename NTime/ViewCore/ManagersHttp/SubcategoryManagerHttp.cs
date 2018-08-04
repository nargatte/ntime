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
    public class SubcategoryManagerHttp : ManagerHttp, ISubcategoryManager
    {
        private IEditableCompetition _currentCompetition;
        private SubcategoryClient _client;
        public ObservableCollection<EditableSubcategory> DefinedSubcategory { get; set; } = new ObservableCollection<EditableSubcategory>();

        public SubcategoryManagerHttp(IEditableCompetition currentCompetition, AccountInfo accountInfo, ConnectionInfo connectionInfo) : base(accountInfo, connectionInfo)
        {
            _currentCompetition = currentCompetition;
            _client = new SubcategoryClient(accountInfo, connectionInfo, "Subcategories");
        }

        public async Task<ObservableCollection<EditableSubcategory>> DownloadSubcategoryAsync()
        {
            await TryCallApi(async () =>
            {
                var dtoSubcategories = await _client.GetAllFromCompetitionAsync(_currentCompetition.DbEntity.Id);
                DefinedSubcategory.Clear();
                foreach (var dtoSubcategory in dtoSubcategories)
                {
                    DefinedSubcategory.Add(new EditableSubcategory(_currentCompetition)
                    {
                        DbEntity = dtoSubcategory.CopyDataFromDto(new Subcategory())
                    });
                }
                DefinedSubcategory.Add(new EditableSubcategory(_currentCompetition)
                {
                    DbEntity = new Subcategory()
                });
            });
            return DefinedSubcategory;
        }
    }
}
