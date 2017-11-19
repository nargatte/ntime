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

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel<Player>
    {
        public PlayersViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            AddPlayerCmd = new RelayCommand(OnAddPlayer);
            TabTitle = "Zawodnicy";
            _newPlayer = new ViewCore.Entities.EditablePlayer(currentCompetition);
        }

        private void OnViewLoaded()
        {
            DownloadAllPlayers();
        }

        private void DownloadAllPlayers()
        {
            DownloadPlayersFromDataBase(new PlayerFilterOptions(), 1, 2000);
        }

        private async void DownloadPlayersFromDataBase(PlayerFilterOptions playerFilter, int pageNumber, int numberOfItemsOnPage)
        {
            var dbPlayersTuple = await _playerRepository.GetAllByFilterAsync(playerFilter,pageNumber, numberOfItemsOnPage);
            var dbPlayers = dbPlayersTuple.Item1;
            foreach (var dbPlayer in dbPlayers)
            {
                Players.Add(new ViewCore.Entities.EditablePlayer(_currentCompetition) { DbEntity = dbPlayer });
            }
        }

        private void OnAddPlayer()
        {
            Players.Add(NewPlayer);
            NewPlayer = new ViewCore.Entities.EditablePlayer(_currentCompetition);
        }

        private ViewCore.Entities.EditablePlayer _newPlayer;
        public ViewCore.Entities.EditablePlayer NewPlayer
        {
            get { return _newPlayer; }
            set { SetProperty(ref _newPlayer, value); }
        }


        private ObservableCollection<ViewCore.Entities.EditablePlayer> _players = new ObservableCollection<ViewCore.Entities.EditablePlayer>();
        public ObservableCollection<ViewCore.Entities.EditablePlayer> Players
        {
            get { return _players; }
            set { SetProperty(ref _players, value); }
        }

        public RelayCommand AddPlayerCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
    }
}
