using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Players
{
    class PlayersViewModel : TabItemViewModel, IViewModel
    {
        public PlayersViewModel()
        {
            Players = new ObservableCollection<BaseCore.DataBase.Player>();
            FillPlayersCollection();
            TabTitle = "Zawodnicy";
        }

        private void FillPlayersCollection()
        {
            for (int i = 0; i < 200; i++)
            {
                Players.Add(new BaseCore.DataBase.Player()
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

        private ObservableCollection<BaseCore.DataBase.Player> _players;
        public ObservableCollection<BaseCore.DataBase.Player> Players
        {
            get { return _players; }
            set { SetProperty(ref _players, value); }
        }
    }
}
