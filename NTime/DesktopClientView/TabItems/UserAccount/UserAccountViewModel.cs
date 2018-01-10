using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;

namespace DesktopClientView.TabItems.UserAccount
{
    public class UserAccountViewModel : PlayersViewModelBase, ICompetitionChoiceBase
    {
        private AccountInfo _user;
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;
        public UserAccountViewModel(IEditableCompetition currentCompetition,
                                    IPlayerManagerFactory playerManagerFactory, IDistanceManagerFactory distanceManagerFactory,
                                    IExtraPlayerInfoManagerFactory extraPlayerInfoManagerFactory, IAgeCategoryManagerFactory ageCategoryManagerFactory,
                                    AccountInfo user, ConnectionInfo connectionInfo)
                                    : base(currentCompetition, playerManagerFactory, distanceManagerFactory, extraPlayerInfoManagerFactory, ageCategoryManagerFactory)
        {
            _competitionData = new CompetitionChoiceBase();
            _user = user;
        }

        public void DetachAllEvents()
        {
            CompetitionData.DetachAllEvents();
            Delegate[] clientList = UserLoginViewReuqested.GetInvocationList();
            foreach (var deleg in clientList)
                UserLoginViewReuqested -= (deleg as Action);
        }

        #region Methods end events
        public event Action UserLoginViewReuqested = delegate { };

        #endregion
    }
}
