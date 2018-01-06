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
    

    public class MainUserViewModel : BindableBase, ITabItemViewModel, ISwitchableViewModel
    {
        private UserAccountViewModel _userAccountViewModel;
        private UserLoginViewModel _userLoginViewModel;
        private AccountInfo _user;
        private ConnectionInfo _connectionInfo;

        public MainUserViewModel(AccountInfo user, ConnectionInfo connectionInfo)
        {
            TabTitle = "Moje konto";
            _user = user;
            _connectionInfo = connectionInfo;
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
        }

        #region Properties
        private ISwitchableViewModel _currentViewModel;
        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public string TabTitle { get; set; }

        #endregion

        #region Methods and events
        public RelayCommand ViewLoadedCmd { get; set; }

        private void OnViewLoaded()
        {
            NavToProperTab();
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        private void NavToProperTab()
        {
            if (string.IsNullOrWhiteSpace(_user.Token))
                NavToUserLoginView();
            else
                NavToUserAccountView();
        }

        private void NavToUserLoginView()
        {
            CurrentViewModel?.DetachAllEvents();
            _userLoginViewModel = new UserLoginViewModel(_user, _connectionInfo);
            _userLoginViewModel.UserAccountViewRequested += NavToUserAccountView;
            CurrentViewModel = _userLoginViewModel;
        }

        private void NavToUserAccountView()
        {
            CurrentViewModel?.DetachAllEvents();
            _userAccountViewModel = new UserAccountViewModel(_user, _connectionInfo);
            _userAccountViewModel.UserLoginViewReuqested += NavToUserLoginView;
            CurrentViewModel = _userAccountViewModel;
        }

        #endregion
    }
}
