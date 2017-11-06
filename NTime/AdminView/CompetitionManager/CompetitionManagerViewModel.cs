using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.CompetitionManager
{
    class CompetitionManagerViewModel : BindableBase, IViewModel
    {
        public CompetitionManagerViewModel()
        {
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
        }

        private void OnGoToCompetition()
        {
            
        }

        private bool CanGoToCompetition()
        {
            return true;
        }

        public void DetachAllEvents()
        {

        }

        public RelayCommand GoToCompetitionCmd { get; private set; }
    }
}
