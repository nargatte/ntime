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
    class UserLoginViewModel : BindableBase, ISwitchableViewModel
    {
        private AccountInfo _user;

        public UserLoginViewModel(AccountInfo user)
        {
            LogInCmd = new RelayCommand(OnLogInRequest);
            RegisterCmd = new RelayCommand(OnRegisterRequested);
            _user = user;
        }

        private void OnRegisterRequested()
        {
            throw new NotImplementedException();
        }

        private void OnLogInRequest()
        {
            UserAccountViewRequested();
        }

        public void DetachAllEvents()
        {
            Delegate[] clientList = UserAccountViewRequested.GetInvocationList();
            foreach (var deleg in clientList)
                UserAccountViewRequested -= (deleg as Action);
        }

        #region Methods and Events
        public RelayCommand LogInCmd { get; private set; }
        public RelayCommand RegisterCmd { get; private set; }

        public event Action UserAccountViewRequested = delegate { };



        #endregion
    }
}
