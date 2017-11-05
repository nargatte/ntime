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
    class MainViewModel : BindableBase
    {
        private BindableBase _currentViewModel;
        private CompetitionChoiceViewModel _competitionChoiceViewModel;
        private PlayersViewModel _playersViewModel;
        private SettingsViewModel _settingsViewModel;
        private ScoresViewModel _scoresViewModel;
        private AddCompetition.AddCompetitionViewModel _addCompetitionViewModel;


        public MainViewModel()
        {
            _competitionChoiceViewModel = new CompetitionChoiceViewModel();
            _addCompetitionViewModel = new AddCompetition.AddCompetitionViewModel();
            CurrentViewModel = _addCompetitionViewModel;
        }


        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }


    }
}
