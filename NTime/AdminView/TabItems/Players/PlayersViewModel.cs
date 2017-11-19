using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using ViewCore.Entities;

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel<Player>
    {
        public PlayersViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoadedAsync);
            AddPlayerCmd = new RelayCommand(OnAddPlayer);
            TabTitle = "Zawodnicy";
        }

        private async void OnViewLoadedAsync()
        {
            await DownloadDistancesAsync();
            await DownloadExtraPlayerInfoAsync();
            await DownloadAllPlayers();
            _newPlayer = new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo);
        }

        private async Task DownloadDistancesAsync()
        {
            var dbDistances = await _distanceRepository.GetAllAsync();
            if (dbDistances.Length > 0)
                foreach (var dbDistance in dbDistances)
                {
                    DefinedDistances.Add(new EditableDistance(_currentCompetition)
                    {
                        DbEntity = dbDistance
                    });
                }
        }

        private async Task DownloadExtraPlayerInfoAsync()
        {
            var dbExtraPlayerInfos = await _extraPlayerInfoRepository.GetAllAsync();
            foreach (var item in dbExtraPlayerInfos)
            {
                DefinedExtraPlayerInfo.Add(new EditableExtraPlayerInfo(_currentCompetition)
                {
                    DbEntity = item
                }
                );
            }
        }

        private async Task DownloadAllPlayers()
        {
            //await DownloadPlayersFromDataBase(new PlayerFilterOptions(), 1, 2000);
            var dbPlayers = await _playerRepository.GetAllAsync();
            AddDbPlayersToGUI(dbPlayers);
        }

        private async Task DownloadPlayersFromDataBase(PlayerFilterOptions playerFilter, int pageNumber, int numberOfItemsOnPage)
        {
            var dbPlayersTuple = await _playerRepository.GetAllByFilterAsync(playerFilter, pageNumber, numberOfItemsOnPage);
            var dbPlayers = dbPlayersTuple.Item1;
            AddDbPlayersToGUI(dbPlayers);
        }

        private void AddDbPlayersToGUI(Player[] dbPlayers)
        {
            foreach (var dbPlayer in dbPlayers)
            {
                Players.Add(new EditablePlayer(_currentCompetition, DefinedDistances, DefinedExtraPlayerInfo)
                {
                    DbEntity = dbPlayer,
                    Distance = new EditableDistance(_currentCompetition),
                    ExtraPlayerInfo = new EditableExtraPlayerInfo(_currentCompetition)
                });
            }
        }

        private void OnAddPlayer()
        {
            Players.Add(NewPlayer);
            NewPlayer = new EditablePlayer(_currentCompetition);
        }

        private EditablePlayer _newPlayer;
        public EditablePlayer NewPlayer
        {
            get { return _newPlayer; }
            set { SetProperty(ref _newPlayer, value); }
        }

        private ObservableCollection<EditablePlayer> _players = new ObservableCollection<EditablePlayer>();
        public ObservableCollection<EditablePlayer> Players
        {
            get { return _players; }
            set { SetProperty(ref _players, value); }
        }



        private ObservableCollection<EditableDistance> _definedDistances = new ObservableCollection<EditableDistance>();
        public ObservableCollection<EditableDistance> DefinedDistances
        {
            get { return _definedDistances; }
            set { SetProperty(ref _definedDistances, value); }
        }


        private ObservableCollection<EditableExtraPlayerInfo> _definedExtraPlayerInfo = new ObservableCollection<EditableExtraPlayerInfo>();
        public ObservableCollection<EditableExtraPlayerInfo> DefinedExtraPlayerInfo
        {
            get { return _definedExtraPlayerInfo; }
            set { SetProperty(ref _definedExtraPlayerInfo, value); }
        }

        public RelayCommand AddPlayerCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
    }
}
