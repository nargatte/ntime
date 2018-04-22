using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            SaveCompetitionDataCmd = new RelayCommand(OnSaveCompetitionData);
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
        }

        #region Properties


        private PlayerAccount _loggedInPlayer = new PlayerAccount();
        public PlayerAccount LoggedInPlayer
        {
            get { return _loggedInPlayer; }
            set { SetProperty(ref _loggedInPlayer, value); }
        }

        private EditablePlayer _fromCompetitonPlayer;
        public EditablePlayer FromCompetitonPlayer
        {
            get { return _fromCompetitonPlayer; }
            set { SetProperty(ref _fromCompetitonPlayer, value); }
        }


        #endregion


        #region Methods end events
        public event Action UserLoginViewReuqested = delegate { };
        public RelayCommand SaveTemplateDataCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand SaveCompetitionDataCmd { get; set; }


        private void OnViewLoaded()
        {
            ClearNewPlayer();
            DownloadInitialData();
        }
        private void OnSaveTemplateData()
        {
            _playerAccountManager.UpdatePlayerTemplateData(LoggedInPlayer);
        }

        private void OnSaveCompetitionData()
        {
            if (CompetitionData.IsCompetitionSelected && FromCompetitonPlayer != null && _playersManager != null)
            {
                _playersManager.UpdatePlayerRegisterInfo(FromCompetitonPlayer.DbEntity);
            }
            else
            {
                MessageBox.Show("Nie udało się zapisać danych");
            }
        }

        private async void DownloadInitialData()
        {
            LoggedInPlayer = await _playerAccountManager.DownloadPlayerTemplateData();
            CompetitionData.DownloadCompetitionsForPlayerAccount(OnlyWithRegistrationEnabled: true);
            CompetitionData.CompetitionSelected += OnCompetitionSelectedAsync;
        }

        private async void OnCompetitionSelectedAsync()
        {
            if (CompetitionData.IsCompetitionSelected)
            {

                await DownloadPlayersInfo(CompetitionData.SelectedCompetition);
                _playersManager = _dependencyContainer.PlayerManagerFactory.CreateInstance(CompetitionData.SelectedCompetition,
                    DefinedDistances, DefinedExtraPlayerInfo, null, _dependencyContainer.User, _dependencyContainer.ConnectionInfo);
                //await _playersManager.AddPlayersFromDatabase(removeAllDisplayedBefore: true);
                //var players = _playersManager.GetPlayersToDisplay();
                //FromCompetitonPlayer = players.FirstOrDefault(p => p.)
                //DisplayPlayerData();
                FromCompetitonPlayer = await _playersManager.GetFullRegisteredPlayerFromCompetition(CompetitionData.SelectedCompetition.DbEntity, LoggedInPlayer);
                if (FromCompetitonPlayer != null)
                {
                    DisplayPlayerData();
                }
            }
            else
            {
                MessageBox.Show("Najpeirw musisz wybrać zawody");
            }
        }

        private void DisplayPlayerData()
        {
            FromCompetitonPlayer.DefinedDistances = DefinedDistances;
            FromCompetitonPlayer.DefinedExtraPlayerInfo = DefinedExtraPlayerInfo;
            FromCompetitonPlayer.Distance = DefinedDistances.FirstOrDefault(defined => defined.DbEntity.Id == FromCompetitonPlayer.DbEntity.DistanceId);
            FromCompetitonPlayer.ExtraPlayerInfo = DefinedExtraPlayerInfo.FirstOrDefault(defined => defined.DbEntity.Id == FromCompetitonPlayer.DbEntity.SubcategoryId);
        }

        private void ClearNewPlayer()
        {
            FromCompetitonPlayer = new EditablePlayer(new EditableCompetition())
            {
                Distance = new EditableDistance(new EditableCompetition()),
                ExtraPlayerInfo = new EditableExtraPlayerInfo(new EditableCompetition()),
                DefinedDistances = new ObservableCollection<EditableDistance>(),
                DefinedExtraPlayerInfo = new ObservableCollection<EditableExtraPlayerInfo>()
            };
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
