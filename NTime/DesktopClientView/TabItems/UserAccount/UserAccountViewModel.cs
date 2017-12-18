using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore;
using ViewCore.Entities;

namespace DesktopClientView.TabItems.UserAccount
{
    public class UserAccountViewModel : ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;
        public string TabTitle { get; set; }
        public UserAccountViewModel()
        {
            _competitionData = new CompetitionChoiceBase();
            TabTitle = "Moje konto";
        }

        public void DetachAllEvents()
        {
            
        }
    }
}
