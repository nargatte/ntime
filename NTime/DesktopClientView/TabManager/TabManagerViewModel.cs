using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView.CompetitionManager;
using DesktopClientView.TabItems.PlayersList;
using DesktopClientView.TabItems.Registration;
using DesktopClientView.TabItems.UserAccount;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;

namespace DesktopClientView.TabManager
{
    public class TabManagerViewModel : CompetitionManagerViewModelBase
    {
        private ConnectionInfo _connectionInfo;
        MainUserViewModel _mainUserViewModel;

        public TabManagerViewModel(AccountInfo accountInfo, ConnectionInfo connectionInfo)
        {
            User = accountInfo;
            _connectionInfo = connectionInfo;
            LogoutCmd = new RelayCommand(OnLogutRequested);
            _mainUserViewModel = new MainUserViewModel(User, _connectionInfo);
            _mainUserViewModel.UserChanged += OnUserChanged;
            TabItems = new System.Collections.ObjectModel.ObservableCollection<ITabItemViewModel>()
            {
                new PlayersListViewModel(User, _connectionInfo), new RegistrationViewModel(User, _connectionInfo), _mainUserViewModel
            };
        }


        #region Properties
        private AccountInfo _user;
        public AccountInfo User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }


        #endregion

        #region Methods and events

        public RelayCommand LogoutCmd { get; private set; }

        private void OnUserChanged()
        {
            OnPropertyChanged(nameof(User));
        }


        private void OnLogutRequested()
        {
            _mainUserViewModel.Logout();
        }

        public override void DetachAllEvents()
        {
            
        }

        #endregion
    }
}
