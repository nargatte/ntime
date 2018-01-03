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
        public MainUserViewModel(AccountInfo user)
        {
            TabTitle = "Moje konto";
            _user = user;
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
            if (_user == null)
                NavToUserLoginView();
            else
                NavToUserAccountView();
        }

        private void NavToUserLoginView()
        {
            CurrentViewModel?.DetachAllEvents();
            _userLoginViewModel = new UserLoginViewModel(_user);
            _userLoginViewModel.UserAccountViewRequested += NavToUserAccountView;
            CurrentViewModel = _userLoginViewModel;
        }

        private void NavToUserAccountView()
        {
            CurrentViewModel?.DetachAllEvents();
            _userAccountViewModel = new UserAccountViewModel(_user);
            _userAccountViewModel.UserLoginViewReuqested += NavToUserLoginView;
            CurrentViewModel = _userAccountViewModel;
        }

        #endregion
    }
}
