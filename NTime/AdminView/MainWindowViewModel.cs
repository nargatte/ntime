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
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Competitions;
using ViewCore.Factories.Distances;
using ViewCore.Factories.ExtraPlayerInfos;
using ViewCore.Factories.Players;

namespace AdminView
{
    class MainWindowViewModel : BindableBase
    {
        private ISwitchableViewModel _currentViewModel;
        private CompetitionChoiceViewModel _competitionChoiceViewModel;
        private CompetitionManagerViewModel _competitionManagerViewModel;
        private IEditableCompetition _currentCompetition;

        private DependencyContainer dependencyContainer;

        public MainWindowViewModel()
        {
            PrepareDependencied();
            NavToCompetitionChoiceView();
            ChangeCompetitionCmd = new RelayCommand(OnChangeCompetition);
            //NavToCompetitionManagerView();
        }

        private void PrepareDependencied()
        {
            var ageCategoryManagerFactory = new AgeCategoryManagerFactoryDesktop();
            var competitionManagerFactory = new CompetitionManagerFactoryDesktop();
            var distanceManagerFactory = new DistanceManagerFactoryDesktop();
            var extraPlayerInfoManagerFactory = new ExtraPlayerInfoManagerFactoryDesktop();
            var playerManagerFactory = new PlayerManagerFactoryDesktop();
            dependencyContainer = new DependencyContainer(ageCategoryManagerFactory, competitionManagerFactory, distanceManagerFactory,
                extraPlayerInfoManagerFactory, playerManagerFactory, null);
        }

        private void OnChangeCompetition()
        {
            NavToCompetitionChoiceView();
        }

        private void NavToCompetitionChoiceView()
        {
            CurrentViewModel?.DetachAllEvents();
            _competitionChoiceViewModel = new CompetitionChoiceViewModel(dependencyContainer);
            _competitionChoiceViewModel.CompetitionManagerRequested += NavToCompetitionManagerView;
            CurrentViewModel = _competitionChoiceViewModel;
        }

        private void NavToCompetitionManagerView()
        {
            if (_competitionChoiceViewModel != null)
                _currentCompetition = _competitionChoiceViewModel.CompetitionData.SelectedCompetition;
            CurrentViewModel?.DetachAllEvents();
            _competitionManagerViewModel = new CompetitionManagerViewModel(_currentCompetition, dependencyContainer);
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
