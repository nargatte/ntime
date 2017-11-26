using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.Entities;
using BaseCore.DataBase;

namespace ViewCore.Managers
{
    public class ExtraPlayerInfosManager : CompetitionItemBase
    {
        public ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo { get; set; } = new ObservableCollection<EditableExtraPlayerInfo>();
        private ExtraPlayerInfoRepository _extraPlayerInfoRepository;
        public ExtraPlayerInfosManager(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
            //_playerRepository
            _extraPlayerInfoRepository = new ExtraPlayerInfoRepository(new ContextProvider(), _currentCompetition.DbEntity);
        }


        public async Task<ObservableCollection<EditableExtraPlayerInfo>> DownloadExtraPlayerInfoAsync()
        {
            var dbExtraPlayerInfos = await _extraPlayerInfoRepository.GetAllAsync();
            DefinedExtraPlayerInfo.Clear();
            foreach (var item in dbExtraPlayerInfos)
            {
                DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
                {
                    DbEntity = item
                });
            }
            return DefinedExtraPlayerInfo;
        }
    }
}
