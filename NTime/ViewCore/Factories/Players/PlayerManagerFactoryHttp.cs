using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.ManagersHttp;
using ViewCore.ManagersInterfaces;
using ViewCore.Entities;
using System.Collections.ObjectModel;
using ViewCore.ManagersDesktop;

namespace ViewCore.Factories.Players
{
    public class PlayerManagerFactoryHttp : IPlayerManagerFactory
    {
        public IPlayerManager CreateInstance(IEditableCompetition currentCompetition, ObservableCollection<EditableDistance> definedDistances,
                                                ObservableCollection<EditableExtraPlayerInfo> definedExtraPlayerInfos, RangeInfo recordsRangeInfo,
                                                AccountInfo accountInfo = null, ConnectionInfo connectionInfo = null)
        {
            return new PlayerManagerHttp(currentCompetition, definedDistances, definedExtraPlayerInfos, recordsRangeInfo, accountInfo, connectionInfo);
        }
    }
}
