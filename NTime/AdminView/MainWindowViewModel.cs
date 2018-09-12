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
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;
using System.Windows;
using BaseCore.Csv.CompetitionSeries;
using System.IO;
using BaseCore.Csv;
using Microsoft.Win32;
using System.Windows.Forms;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;
using MessageBox = System.Windows.Forms.MessageBox;

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
            var subcategoryManagerFactory = new SubcategoryManagerFactoryDesktop();
            var playerManagerFactory = new PlayerManagerFactoryDesktop();
            dependencyContainer = new DependencyContainer(ageCategoryManagerFactory, competitionManagerFactory, distanceManagerFactory,
                subcategoryManagerFactory, playerManagerFactory, null);
        }

        private void OnChangeCompetition()
        {
            NavToCompetitionChoiceView();
        }

        private async void OnCalculateStandings()
        {
            var filesDictionary = new Dictionary<string, string>();
            filesDictionary.Add("klasyki_punkty.csv", Properties.Resources.klasyki_punkty);
            var competitionNames = new List<string> { "Aleksandrow", "Zdunska", "Pabianice1", "Buczek", "Pabianice2" };
            var competitionNamesDict = new Dictionary<int, string>();
            int iter = 0;
            competitionNames.ForEach(name => competitionNamesDict.Add(iter++, name));

            var resourceLoader = new ResourceLoader();
            var competitionPaths = new List<string>();
            //competitionPaths.AddRange(resourceLoader.LoadFilesToTemp(filesDictionary.Skip(1).ToDictionary(x => x.Key, x => x.Value), Path.GetTempPath()));
            var dialog = new OpenFileDialog();
            dialog.Filter = "csv files (*.csv)|*.csv";
            dialog.RestoreDirectory = true;
            dialog.Title = "Wybierz w dobrej kolejności wyniki dla cyklu";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    competitionPaths = dialog.FileNames.ToList();
                    var seriesStandings = new SeriesStandings(competitionNamesDict);
                    await seriesStandings.ImportScoresFromCsv(competitionPaths);

                    var pointsPath = resourceLoader.LoadFilesToTemp(filesDictionary.Take(1).ToDictionary(x => x.Key, x => x.Value), Path.GetTempPath());
                    await seriesStandings.ImportPointsTableFromCsv(pointsPath.First());

                    seriesStandings.CalculateResults();

                    MessageBox.Show("Wyniki rankingu zostały przeliczone");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Nie udało sie przeliczyć wyników. Błąd: " + e.Message);
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano żadnych zawodów");
            }
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
