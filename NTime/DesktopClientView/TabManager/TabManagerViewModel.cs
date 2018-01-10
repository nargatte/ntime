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
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;

namespace DesktopClientView.TabManager
{
    public class TabManagerViewModel : CompetitionManagerViewModelBase
    {
        //private ConnectionInfo _connectionInfo;
        MainUserViewModel _mainUserViewModel;

        //public TabManagerViewModel(IPlayerManagerFactory playerManagerFactory, IDistanceManagerFactory distanceManagerFactory,
        //                            IExtraPlayerInfoManagerFactory extraPlayerInfoManagerFactory, IAgeCategoryManagerFactory ageCategoryManagerFactory,
        //                            AccountInfo user, ConnectionInfo connectionInfo)
        //{
        //    User = user;
        //    _connectionInfo = connectionInfo;
        //    LogoutCmd = new RelayCommand(OnLogutRequested);
        //    _mainUserViewModel = new MainUserViewModel(playerManagerFactory, distanceManagerFactory, extraPlayerInfoManagerFactory, ageCategoryManagerFactory, User, _connectionInfo);
        //    _mainUserViewModel.UserChanged += OnUserChanged;
        //    TabItems = new System.Collections.ObjectModel.ObservableCollection<ITabItemViewModel>()
        //    {
        //        new PlayersListViewModel(new EditableCompetition(), playerManagerFactory, distanceManagerFactory, extraPlayerInfoManagerFactory, ageCategoryManagerFactory, User, _connectionInfo),
        //        new RegistrationViewModel(new EditableCompetition(), playerManagerFactory, distanceManagerFactory, extraPlayerInfoManagerFactory, ageCategoryManagerFactory, User, _connectionInfo),
        //        _mainUserViewModel
        //    };
        //}

        public TabManagerViewModel(DependencyContainer dependencyContainer)
        {
            User = dependencyContainer.User;
            //_connectionInfo = connectionInfo;
            LogoutCmd = new RelayCommand(OnLogutRequested);
            _mainUserViewModel = new MainUserViewModel(dependencyContainer);
            _mainUserViewModel.UserChanged += OnUserChanged;
            TabItems = new System.Collections.ObjectModel.ObservableCollection<ITabItemViewModel>()
            {
                new PlayersListViewModel(new EditableCompetition(), dependencyContainer),
                new RegistrationViewModel(new EditableCompetition(), dependencyContainer),
                _mainUserViewModel
            };
        }


        #region Properties
        private AccountInfo _user;
        private DependencyContainer dependencyContainer;

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
