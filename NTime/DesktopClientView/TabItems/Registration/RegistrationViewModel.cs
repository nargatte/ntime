using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;

namespace DesktopClientView.TabItems.Registration
{
    public class RegistrationViewModel : PlayersViewModelBase, ICompetitionChoiceBase 
    {
        AccountInfo _user;
        private ConnectionInfo _connectionInfo;
        private CompetitionChoiceBase _competitionData;
        public CompetitionChoiceBase CompetitionData => _competitionData;
        public RegistrationViewModel(AccountInfo user, ConnectionInfo connectionInfo)
        {
            TabTitle = "Zapisy";
            _user = user;
            _connectionInfo = connectionInfo;
            _competitionData = new CompetitionChoiceBase();
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            AddPlayerCmd = new RelayCommand(OnAddPlayerAsync);
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
            CompetitionData.DownloadCompetitionsFromDatabaseAndDisplay();
            CompetitionData.CompetitionSelected += OnCompetitionSelectedAsync;
        }

        private async void OnCompetitionSelectedAsync()
        {
            if (CompetitionData.IsCompetitionSelected)
            {
                await DownloadPlayersInfo(CompetitionData.SelectedCompetition);
                ClearNewPlayer();
            }
        }

        public void DetachAllEvents()
        {
            CompetitionData.DetachAllEvents();
        }


        private async void OnAddPlayerAsync()
        {
            if (await _playersManager.TryAddPlayerAsync(NewPlayer))
            {
                Players = _playersManager.GetPlayersToDisplay();
                UpdateRecordsRangeInfo(_playersManager.GetRecordsRangeInfo());
                ClearNewPlayer();
            }
        }

        private void ClearNewPlayer()
        {
            NewPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo, new Player()
            {
                Distance = new Distance(),
                ExtraPlayerInfo = new ExtraPlayerInfo(),
                StartTime = DateTime.Today,
                BirthDate = DateTime.Today
            });
        }

        #endregion
    }
}
