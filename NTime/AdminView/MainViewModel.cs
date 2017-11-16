using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView.CompetitionChoice;
using AdminView.Players;
using AdminView.Scores;
using AdminView.Settings;
using AdminView.CompetitionManager;
using ViewCore;
using MvvmHelper;

namespace AdminView
{
    class MainViewModel : BindableBase, IViewModel
    {
        private IViewModel _currentViewModel;
        private CompetitionChoiceViewModel _competitionChoiceViewModel;
        private CompetitionManagerViewModel _competitionManagerViewModel;
        private ViewCore.Entities.EditableCompetition _currentCompetition;


        public MainViewModel()
        {

            //NavToCompetitionChoiceView();
            NavToCompetitionManager();
        }

        private void NavToCompetitionChoiceView()
        {
            CurrentViewModel?.DetachAllEvents();
            _competitionChoiceViewModel = new CompetitionChoiceViewModel();
            _competitionChoiceViewModel.CompetitionManagerRequested += NavToCompetitionManager;
            CurrentViewModel = _competitionChoiceViewModel;
        }

        private void NavToCompetitionManager()
        {
            if (_competitionChoiceViewModel != null)
                _currentCompetition = _competitionChoiceViewModel.SelectedCompetition;
            CurrentViewModel?.DetachAllEvents();
            _competitionManagerViewModel = new CompetitionManagerViewModel(_currentCompetition);
            CurrentViewModel = _competitionManagerViewModel;
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
