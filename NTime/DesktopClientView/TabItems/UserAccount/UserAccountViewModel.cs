using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;

namespace DesktopClientView.TabItems.UserAccount
{
    public class UserAccountViewModel : PlayersViewModelBase, ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;

        public UserAccountViewModel(EditableCompetition currentCompetition, DependencyContainer dependencyContainer) : base(currentCompetition, dependencyContainer)
        {
            _competitionData = new CompetitionChoiceBase(dependencyContainer);
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
