using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView.CompetitionManager;
using DesktopClientView.TabItems.PlayersList;
using DesktopClientView.TabItems.Registration;
using DesktopClientView.TabItems.UserAccount;
using ViewCore;
using ViewCore.Entities;

namespace DesktopClientView.CompetitionManager
{
    class CompetitionManagerViewModel : CompetitionManagerViewModelBase
    {
        public CompetitionManagerViewModel()
        {
            TabItems = new System.Collections.ObjectModel.ObservableCollection<ITabItemViewModel>()
            {
                new PlayersListViewModel(User), new RegistrationViewModel(User), new MainUserViewModel(User)
            };
        }


        private AccountInfo _user;
        public AccountInfo User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public override void DetachAllEvents()
        {
            
        }
    }
}
