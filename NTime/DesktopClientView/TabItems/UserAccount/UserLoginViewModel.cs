using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.ManagersHttp;

namespace DesktopClientView.TabItems.UserAccount
{
    class UserLoginViewModel : BindableBase, ISwitchableViewModel
    {
        private AccountInfo _user;
        private ConnectionInfo _connectionInfo;
        private AccountManagerHttp _accountManager;

        public UserLoginViewModel(AccountInfo user, ConnectionInfo connectionInfo)
        {
            LogInCmd = new RelayCommand(OnLogInRequest);
            RegisterCmd = new RelayCommand(OnRegisterRequested);
            _user = user;
            _connectionInfo = connectionInfo;
            _accountManager = new AccountManagerHttp(_user, _connectionInfo);
            LoginPasswordChangedCommand = new RelayCommand<PasswordBox>(OnLoginPasswordChanged);
            RegistrationPasswordChangedCommand = new RelayCommand<PasswordBox>(OnRegistrationPasswordChanged);
            RegistrationConfirmPasswordChangedCommand = new RelayCommand<PasswordBox>(OnRegistrationConfirmPasswordChanged);
        }


        #region Properties

        private string _loginEmail;
        public string LoginEmail
        {
            get { return _loginEmail; }
            set { SetProperty(ref _loginEmail, value); }
        }

        private string _loginPassword;
        public string LoginPassword
        {
            get { return _loginPassword; }
            set { SetProperty(ref _loginPassword, value); }
        }

        private string _registrationEmail;
        public string RegistrationEmail
        {
            get { return _registrationEmail; }
            set { SetProperty(ref _registrationEmail, value); }
        }

        private string _registrationPassword;
        public string RegistrationPassword
        {
            get { return _registrationPassword; }
            set { SetProperty(ref _registrationPassword, value); }
        }

        private string _registrationConfirmPassword;
        public string RegistrationConfirmPassword
        {
            get { return _registrationConfirmPassword; }
            set { SetProperty(ref _registrationConfirmPassword, value); }
        }

        #endregion

        #region Methods and Events
        public RelayCommand LogInCmd { get; private set; }
        public RelayCommand RegisterCmd { get; private set; }
        public RelayCommand<PasswordBox> LoginPasswordChangedCommand { get; private set; }
        public RelayCommand<PasswordBox> RegistrationPasswordChangedCommand { get; private set; }
        public RelayCommand<PasswordBox> RegistrationConfirmPasswordChangedCommand { get; private set; }

        public event Action UserAccountViewRequested = delegate { };

        private async void OnRegisterRequested()
        {
            bool isSuccess = await _accountManager.Register(RegistrationEmail, RegistrationPassword, RegistrationConfirmPassword);
            if (isSuccess)
            {
                DisplayNotification("Twoje konto zostało utworzone. Możesz się zalogować");
            }
            else
            {
                DisplayNotification(_accountManager.ExcpetionMessage);
            }
        }

        private async void OnLogInRequest()
        {
            bool isSuccess = await _accountManager.Login(LoginEmail, LoginPassword);
            if (isSuccess)
            {
                UserAccountViewRequested();
            }
            else
            {
                DisplayNotification(_accountManager.ExcpetionMessage);
            }
        }

        private void DisplayNotification(string message)
        {
            System.Windows.MessageBox.Show(message);
        }

        public void DetachAllEvents()
        {
            Delegate[] clientList = UserAccountViewRequested.GetInvocationList();
            foreach (var deleg in clientList)
                UserAccountViewRequested -= (deleg as Action);
        }


        private void OnLoginPasswordChanged(PasswordBox obj)
        {
            LoginPassword = obj.Password;
        }

        private void OnRegistrationPasswordChanged(PasswordBox obj)
        {
            RegistrationPassword = obj.Password;
        }

        private void OnRegistrationConfirmPasswordChanged(PasswordBox obj)
        {
            RegistrationConfirmPassword = obj.Password;
        }

        #endregion
    }
}
