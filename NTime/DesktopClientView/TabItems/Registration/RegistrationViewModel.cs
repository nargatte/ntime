using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using System.Collections.ObjectModel;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;
using ViewCore.ManagersHttp;
using ViewCore.ManagersInterfaces;

namespace DesktopClientView.TabItems.Registration
{
    public class RegistrationViewModel : PlayersViewModelBase, ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;
        private IPlayerAccountManager _playerAccountManager;

        public RegistrationViewModel(EditableCompetition currentCompetition, DependencyContainer dependencyContainer) : base(currentCompetition, dependencyContainer)
        {
            TabTitle = "Zapisy";
            _dependencyContainer = dependencyContainer;
            _competitionData = new CompetitionChoiceBase(dependencyContainer);
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            AddPlayerCmd = new RelayCommand(OnAddPlayerAsync);
            ClearNewPlayer();
        }

        #region Properties

        private EditablePlayer _newPlayer;
        public EditablePlayer NewPlayer
        {
            get { return _newPlayer; }
            set { SetProperty(ref _newPlayer, value); }
        }

        #endregion

        #region Methods and Event

        public RelayCommand ViewLoadedCmd { get; set; }
        public RelayCommand AddPlayerCmd { get; private set; }

        private void OnViewLoaded()
        {
            DetachAllEvents();
            _playerAccountManager = _dependencyContainer.PlayerAccountManagerFactory.CreateInstance(_dependencyContainer);
            CompetitionData.DownloadCompetitionsFromDatabaseAndDisplay(OnlyWithRegistrationEnabled: true);
            CompetitionData.CompetitionSelected += OnCompetitionSelectedAsync;
            DownloadPlayerTemplateDataIfLoggedIn();
        }

        private async void DownloadPlayerTemplateDataIfLoggedIn()
        {
            if (_dependencyContainer.User.IsAuthenticated)
            {
                var templateData = await _playerAccountManager.DownloadPlayerTemplateData();
                if (templateData != null)
                    NewPlayer = new EditablePlayer(new EditableCompetition()) { DbEntity = new Player() };
            }
        }

        private async void OnCompetitionSelectedAsync()
        {
            if (CompetitionData.IsCompetitionSelected)
            {

                await DownloadPlayersInfo(CompetitionData.SelectedCompetition);
                _playersManager = _dependencyContainer.PlayerManagerFactory.CreateInstance(CompetitionData.SelectedCompetition,
                    DefinedDistances, DefinedExtraPlayerInfo, null, _dependencyContainer.User, _dependencyContainer.ConnectionInfo);
                DisplayPlayerData();
            }
            else
            {
                MessageBox.Show("Najpeirw musisz wybrać zawody");
            }
        }

        public void DetachAllEvents()
        {
            CompetitionData.DetachAllEvents();
        }

        private void DisplayPlayerData()
        {
            NewPlayer.DefinedDistances = DefinedDistances;
            NewPlayer.DefinedExtraPlayerInfo = DefinedExtraPlayerInfo;
        }

        private void ClearNewPlayer()
        {
            NewPlayer = new EditablePlayer(new EditableCompetition())
            {
                Distance = new EditableDistance(new EditableCompetition()),
                ExtraPlayerInfo = new EditableExtraPlayerInfo(new EditableCompetition()),
                DefinedDistances = new ObservableCollection<EditableDistance>(),
                DefinedExtraPlayerInfo = new ObservableCollection<EditableExtraPlayerInfo>()
            };
        }

        private async void OnAddPlayerAsync()
        {
            var isRegistrationSuccessfull = await _playersManager.TryAddPlayerAsync(NewPlayer);
        }

        #endregion
    }
}
