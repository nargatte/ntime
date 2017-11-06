using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView.CompetitionChoice;
using AdminView.Players;
using AdminView.Scores;
using AdminView.Settings;

namespace AdminView
{
    class MainViewModel : BindableBase, IViewModel
    {
        private IViewModel _currentViewModel;
        private CompetitionChoiceViewModel _competitionChoiceViewModel;
        private PlayersViewModel _playersViewModel;
        private SettingsViewModel _settingsViewModel;
        private ScoresViewModel _scoresViewModel;


        public MainViewModel()
        {
            NavToCompetitionChoiceView();
        }

        private void NavToCompetitionChoiceView()
        {
            CurrentViewModel?.DetachAllEvents();
            _competitionChoiceViewModel = new CompetitionChoiceViewModel();
            _competitionChoiceViewModel.PlayersViewRequested += NavToPlayersView;
            CurrentViewModel = _competitionChoiceViewModel;
        }

        private void NavToPlayersView()
        {
            CurrentViewModel?.DetachAllEvents();
            _playersViewModel = new PlayersViewModel();
            CurrentViewModel = _playersViewModel;
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        public IViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
    }
}
