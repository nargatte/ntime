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
using AdminView.Logs;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;

namespace AdminView.CompetitionManager
{
    class CompetitionManagerViewModel : CompetitionManagerViewModelBase
    {

        public CompetitionManagerViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            TabItems = new ObservableCollection<ITabItemViewModel>()
            {
                new SettingsViewModel(_currentCompetition), new PlayersViewModel(_currentCompetition), new DistancesViewModel(_currentCompetition),
                new CategoriesViewModel(_currentCompetition), new LogsViewModel(_currentCompetition),new ScoresViewModel(_currentCompetition)
            };
        }

        private void OnGoToCompetition()
        {
            
        }

        private bool CanGoToCompetition()
        {
            return true;
        }

        public override void DetachAllEvents()
        {

        }

        public RelayCommand GoToCompetitionCmd { get; private set; }
    }
}
