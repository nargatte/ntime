using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopClientView.TabManager;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;

namespace DesktopClientView
{
    internal class MainWindowViewModel : BindableBase
    {
        private ISwitchableViewModel _currentViewModel;
        private AccountInfo _accountInfo;
        private ConnectionInfo _connectionInfo;


        public MainWindowViewModel()
        {
            PrepareDependencies();
            CurrentViewModel = new TabManagerViewModel(_accountInfo, _connectionInfo);
        }

        private void PrepareDependencies()
        {
            _accountInfo = new AccountInfo();
            _connectionInfo = new ConnectionInfo() { ServerURL = "http://projektnet.mini.pw.edu.pl/NTime" };
        }

        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
    }
}
