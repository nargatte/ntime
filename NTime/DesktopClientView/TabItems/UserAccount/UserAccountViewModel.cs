using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;
using ViewCore.ManagersInterfaces;

namespace DesktopClientView.TabItems.UserAccount
{
    public class UserAccountViewModel : PlayersViewModelBase, ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionData;
        private IPlayerAccountManager _playerAccountManager;

        public CompetitionChoiceBase CompetitionData => _competitionData;

        public UserAccountViewModel(EditableCompetition currentCompetition, DependencyContainer dependencyContainer) : base(currentCompetition, dependencyContainer)
        {
            _competitionData = new CompetitionChoiceBase(dependencyContainer);
            _playerAccountManager = dependencyContainer.PlayerAccountManagerFactory.CreateInstance(dependencyContainer);
            SaveTemplateDataCmd = new RelayCommand(OnSaveTemplateData);
            DownloadInitialData();
        }

        #region Properties


        private PlayerAccount _loggedInPlayer = new PlayerAccount();
        public PlayerAccount LoggedInPlayer
        {
            get { return _loggedInPlayer; }
            set { SetProperty(ref _loggedInPlayer, value); }
        }

        private PlayerAccount _fromCompetitonPlayer = new PlayerAccount();
        public PlayerAccount FromCompetitonPlayer
        {
            get { return _fromCompetitonPlayer; }
            set { SetProperty(ref _fromCompetitonPlayer, value); }
        }


        #endregion


        #region Methods end events
        public event Action UserLoginViewReuqested = delegate { };
        public RelayCommand SaveTemplateDataCmd { get; private set; }

        private void OnSaveTemplateData()
        {
            _playerAccountManager.UpdatePlayerTemplateData(LoggedInPlayer);
        }

        private async void DownloadInitialData()
        {
            LoggedInPlayer = await _playerAccountManager.DownloadPlayerTemplateData();
        }

        public void DetachAllEvents()
        {
            CompetitionData.DetachAllEvents();
            Delegate[] clientList = UserLoginViewReuqested.GetInvocationList();
            foreach (var deleg in clientList)
                UserLoginViewReuqested -= (deleg as Action);
        }

        #endregion
    }
}
