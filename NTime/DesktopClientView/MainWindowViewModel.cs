using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopClientView.TabManager;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;

namespace DesktopClientView
{
    internal class MainWindowViewModel : BindableBase
    {
        private ISwitchableViewModel _currentViewModel;
        private AccountInfo _user;
        private ConnectionInfo _connectionInfo;

        protected IPlayerManagerFactory _playerManagerFactory;
        protected IDistanceManagerFactory _distanceManagerFactory;
        protected IExtraPlayerInfoManagerFactory _extraPlayerInfoManagerFactory;
        protected IAgeCategoryManagerFactory _ageCategoryManagerFactory;


        public MainWindowViewModel()
        {
            PrepareDependencies();
            CurrentViewModel = new TabManagerViewModel(_playerManagerFactory, _distanceManagerFactory, _extraPlayerInfoManagerFactory, _ageCategoryManagerFactory, _user, _connectionInfo);
        }

        private void PrepareDependencies()
        {
            _user = new AccountInfo();
            _connectionInfo = new ConnectionInfo() { ServerURL = "http://projektnet.mini.pw.edu.pl/NTime" };

            _playerManagerFactory = new PlayerManagerFactoryDesktop();
            _distanceManagerFactory = new DistanceManagerFactoryDesktop();
            _extraPlayerInfoManagerFactory = new ExtraPlayerInfoManagerFactoryDesktop();
            _ageCategoryManagerFactory = new AgeCategoryManagerFactoryDesktop();
        }

        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
    }
}
