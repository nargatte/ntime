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
using ViewCore.Entities;

namespace AdminView
{
    class MainWindowViewModel : BindableBase
    {
        private ISwitchableViewModel _currentViewModel;
        private CompetitionChoiceViewModel _competitionChoiceViewModel;
        private CompetitionManagerViewModel _competitionManagerViewModel;
        private IEditableCompetition _currentCompetition;

        public MainWindowViewModel()
        {
            NavToCompetitionChoiceView();
            ChangeCompetitionCmd = new RelayCommand(OnChangeCompetition);
            //NavToCompetitionManagerView();
        }

        private void OnChangeCompetition()
        {
            NavToCompetitionChoiceView();
        }

        private void NavToCompetitionChoiceView()
        {
            CurrentViewModel?.DetachAllEvents();
            _competitionChoiceViewModel = new CompetitionChoiceViewModel();
            _competitionChoiceViewModel.CompetitionManagerRequested += NavToCompetitionManagerView;
            CurrentViewModel = _competitionChoiceViewModel;
        }

        private void NavToCompetitionManagerView()
        {
            if (_competitionChoiceViewModel != null)
                _currentCompetition = _competitionChoiceViewModel.CompetitionData.SelectedCompetition;
            CurrentViewModel?.DetachAllEvents();
            _competitionManagerViewModel = new CompetitionManagerViewModel(_currentCompetition);
            CurrentViewModel = _competitionManagerViewModel;
        }

        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public RelayCommand ChangeCompetitionCmd { get; private set; }
    }
}
