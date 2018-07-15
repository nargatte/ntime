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

        private void OnCalculateStandings()
        {
            string mainPath = Path.GetTempPath();
            string aleksandrowFileName = "Aleksandrow.csv";
            string zdunskaFileName = "Zdunska.csv";

            Dictionary<string, string> filesDictionary = new Dictionary<string, string>();
            filesDictionary.Add(aleksandrowFileName, Properties.Resources.Aleksandrow);
            filesDictionary.Add(zdunskaFileName, Properties.Resources.Zdunska);

            foreach (KeyValuePair<string, string> dpv in filesDictionary)
            {
                if (File.Exists(mainPath + dpv.Key))
                    File.Delete(mainPath + dpv.Key);

                using (FileStream fs = File.Create(mainPath + dpv.Key))
                {
                    byte[] tb = new UTF8Encoding(true).GetBytes(dpv.Value);
                    fs.Write(tb, 0, tb.Length);
                }
            }

            var seriesStandings = new SeriesStandings();
            var competitions = new List<string>
            {
                mainPath + aleksandrowFileName,
                mainPath + zdunskaFileName,
            };
            seriesStandings.ImportScoresFromCsv(competitions);
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
