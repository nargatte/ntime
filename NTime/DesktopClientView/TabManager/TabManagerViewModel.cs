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
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;

namespace DesktopClientView.TabManager
{
    public class TabManagerViewModel : CompetitionManagerViewModelBase
    {
        private MainUserViewModel _mainUserViewModel;
        private DependencyContainer _dependencyContainer;

        public TabManagerViewModel(DependencyContainer dependencyContainer)
        {
            User = dependencyContainer.User;
            _dependencyContainer = dependencyContainer;
            //_connectionInfo = connectionInfo;
            LogoutCmd = new RelayCommand(OnLogutRequested);
            PrepareTabs(dependencyContainer);
        }

        private void PrepareTabs(DependencyContainer dependencyContainer)
        {
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
            OnPropertyChanged(nameof(User.UserName));
            OnPropertyChanged(nameof(User.IsAuthenticated));
        }


        private async void OnLogutRequested()
        {
            var isSuccess = await _mainUserViewModel.Logout();
            if (isSuccess)
            {
                PrepareTabs(_dependencyContainer);
            }
        }

        public override void DetachAllEvents()
        {
            
        }

        #endregion
    }
}
