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
using System.Windows;
using BaseCore.Csv.CompetitionSeries;
using System.IO;
using BaseCore.Csv;

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
            CalculateStandings = new RelayCommand(OnCalculateStandings);
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

        private async void OnCalculateStandings()
        {
            var filesDictionary = new Dictionary<string, string>();
            filesDictionary.Add("klasyki_punkty.csv", Properties.Resources.klasyki_punkty);
            filesDictionary.Add("Aleksandrow.csv", Properties.Resources.Aleksandrow);
            filesDictionary.Add("Zdunska.csv", Properties.Resources.Zdunska);

            var resourceLoader = new ResourceLoader();
            var competitionPaths = resourceLoader.LoadFilesToTemp(filesDictionary.Skip(1).ToDictionary(x => x.Key, x => x.Value), Path.GetTempPath());
            var seriesStandings = new SeriesStandings();
            await seriesStandings.ImportScoresFromCsv(competitionPaths);

            var pointsPath = resourceLoader.LoadFilesToTemp(filesDictionary.Take(1).ToDictionary(x => x.Key, x => x.Value), Path.GetTempPath());
            await seriesStandings.ImportPointsTableFromCsv(pointsPath.First());

            seriesStandings.CalculateResults();
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
            _competitionManagerViewModel.CompetitionRemoved += OnCompetitionRemoved;
            CurrentViewModel = _competitionManagerViewModel;
        }

        private void OnCompetitionRemoved()
        {
            NavToCompetitionChoiceView();
            MessageBox.Show("Zawody zostały poprawnie usunięte");
        }

        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public RelayCommand ChangeCompetitionCmd { get; private set; }
        public RelayCommand CalculateStandings { get; private set; }
    }
}
