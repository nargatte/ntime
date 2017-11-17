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
using MvvmHelper;

namespace AdminView.CompetitionManager
{
    class CompetitionManagerViewModel : BindableBase, ViewCore.Entities.IViewModel
    {
        private ViewCore.Entities.EditableCompetition _currentCompetition;

        public CompetitionManagerViewModel(ViewCore.Entities.EditableCompetition currentCompetition)
        {
            this._currentCompetition = currentCompetition;
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            TabItems = new ObservableCollection<BindableBase>()
            {
                new SettingsViewModel(_currentCompetition), new PlayersViewModel(_currentCompetition), new DistancesViewModel(_currentCompetition),
                new CategoriesViewModel(_currentCompetition), new ScoresViewModel(_currentCompetition)
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
