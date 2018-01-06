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
    public class CompetitionManagerViewModel : CompetitionManagerViewModelBase
    {
        private ConnectionInfo _connectionInfo;

        public CompetitionManagerViewModel(AccountInfo accountInfo, ConnectionInfo connectionInfo)
        {
            User = accountInfo;
            _connectionInfo = connectionInfo;
            TabItems = new System.Collections.ObjectModel.ObservableCollection<ITabItemViewModel>()
            {
                new PlayersListViewModel(User, _connectionInfo), new RegistrationViewModel(User, _connectionInfo), new MainUserViewModel(User, _connectionInfo)
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
