using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;
using ViewCore.ManagersHttp;

namespace DesktopClientView.TabItems.UserAccount
{
    //This is the whole tab and the view is switched
    //depending on the fact if user is logged in or not
    

    public class MainUserViewModel : BindableBase, ITabItemViewModel, ISwitchableViewModel
    {
        private UserAccountViewModel _userAccountViewModel;
        private UserLoginViewModel _userLoginViewModel;

        private DependencyContainer _dependencyContainer;

        public MainUserViewModel(DependencyContainer dependencyContainer)
        {
            TabTitle = "Moje konto";
            _dependencyContainer = dependencyContainer;
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
        }

        #region Properties
        private ISwitchableViewModel _currentViewModel;
        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public AccountInfo User
        {
            get => _dependencyContainer.User;
            set => _dependencyContainer.User =  SetProperty(_dependencyContainer.User, value);
        }

        public string TabTitle { get; set; }

        #endregion

        #region Methods and events
        public RelayCommand ViewLoadedCmd { get; set; }

        public event Action UserChanged = delegate { };

        private void OnViewLoaded()
        {
            NavToProperTab();
        }

        public void DetachAllEvents()
        {
            
        }

        private void NavToProperTab()
        {
            if (string.IsNullOrWhiteSpace(User.Token))
                NavToUserLoginView();
            else
                NavToUserAccountView();
        }

        internal async Task<bool> Logout()
        {
            var accountManager = new AuthenticationManagerHttp(User, _dependencyContainer.ConnectionInfo);
            bool isSuccess = await accountManager.Logout();
            if (isSuccess)
            {
                DisplayNotifiation("Wylogowanie przebiegło poprawnie");
                NavToUserLoginView();
            }
            else
            {
                DisplayNotifiation(accountManager.ExcpetionMessage);
            }
            return isSuccess;
        }

        private void DisplayNotifiation(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        private void NavToUserLoginView()
        {
            CurrentViewModel?.DetachAllEvents();
            _userLoginViewModel = new UserLoginViewModel(_dependencyContainer);
            _userLoginViewModel.UserAccountViewRequested += NavToUserAccountView;
            _userLoginViewModel.RefreshRequested += OnUserLoginViewRefreshRequested;
            CurrentViewModel = _userLoginViewModel;
            UserChanged?.Invoke();
        }

        private void OnUserLoginViewRefreshRequested()
        {
            //NavToUserAccountView();
            //System.Threading.Thread.Sleep(2000);
            NavToUserLoginView();
        }

        private void NavToUserAccountView()
        {
            CurrentViewModel?.DetachAllEvents();
            _userAccountViewModel = new UserAccountViewModel(new EditableCompetition(), _dependencyContainer);
            _userAccountViewModel.UserLoginViewReuqested += NavToUserLoginView;
            CurrentViewModel = _userAccountViewModel;
            UserChanged();
        }

        #endregion
    }
}
