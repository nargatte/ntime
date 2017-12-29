using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;

namespace DesktopClientView.TabItems.UserAccount
{
    //This is the whole tab and the view is switched
    //depending on the fact if user is logged in or not
    public class MainUserViewModel : BindableBase, ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;


        private ISwitchableViewModel _currentViewModel;


        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
        public MainUserViewModel()
        {
            TabTitle = "Moje konto";
            //CurrentViewModel = ;
        }

        public string TabTitle { get; set; }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }
    }
}
