using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView;
using AdminView.Settings;
using AdminView.Players;
using AdminView.Scores;
using AdminView.Distances;
using AdminView.Categories;

namespace AdminView.CompetitionManager
{
    class CompetitionManagerViewModel : BindableBase, IViewModel
    {
        public CompetitionManagerViewModel()
        {
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            TabItems = new ObservableCollection<BindableBase>()
            {
                new SettingsViewModel(), new PlayersViewModel(), new DistancesViewModel(), new CategoriesViewModel(), new ScoresViewModel()
            };
        }


        private ObservableCollection<BindableBase> _tabItems;
        public ObservableCollection<BindableBase> TabItems
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }


        private void OnGoToCompetition()
        {
            
        }

        private bool CanGoToCompetition()
        {
            return true;
        }

        public void DetachAllEvents()
        {

        }

        public RelayCommand GoToCompetitionCmd { get; private set; }
    }
}
