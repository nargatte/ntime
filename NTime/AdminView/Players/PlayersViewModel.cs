using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore;

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel, IViewModel
    {
        public PlayersViewModel(ViewCore.Entities.EditableCompetition currentCompetition) : base(currentCompetition)
        {
            AddPlayerCmd = new RelayCommand(OnAddPlayer);
            FillPlayersCollection();
            TabTitle = "Zawodnicy";
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

        private void FillPlayersCollection()
        {
            for (int i = 0; i < 200; i++)
            {
                Players.Add(new ViewCore.Entities.EditablePlayer()
                {
                    LastName = "Kierzkowski",
                    FirstName = "Jan",
                    Team = "Niezniszczalni Zgierz",
                    BirthDate = new DateTime(1994, 10, 09),
                    IsMale = true,
                    StartNumber = 18
                });
            }
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        private ObservableCollection<ViewCore.Entities.EditablePlayer> _players = new ObservableCollection<ViewCore.Entities.EditablePlayer>();
        public ObservableCollection<ViewCore.Entities.EditablePlayer> Players
        {
            get { return _players; }
            set { SetProperty(ref _players, value); }
        }

        public RelayCommand AddPlayerCmd { get; private set; }
    }
}
