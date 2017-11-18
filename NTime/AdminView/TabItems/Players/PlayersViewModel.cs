using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel
    {
        public PlayersViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            AddPlayerCmd = new RelayCommand(OnAddPlayer);
            TabTitle = "Zawodnicy";
        }

        private void OnViewLoaded()
        {
            DownloadPlayersFromDataBase();
        }

        private async void DownloadPlayersFromDataBase()
        {
            var repository = new PlayerRepository(new ContextProvider(), _currentCompetition.Competition);
            var dbPlayers = await repository.GetAllAsync();
            foreach (var dbPlayer in dbPlayers)
            {
                Players.Add(new ViewCore.Entities.EditablePlayer() { Player = dbPlayer });
            }
        }

        private void OnAddPlayer()
        {
            Players.Add(NewPlayer);
            NewPlayer = new ViewCore.Entities.EditablePlayer();
        }

        private ViewCore.Entities.EditablePlayer _newPlayer = new ViewCore.Entities.EditablePlayer();
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
