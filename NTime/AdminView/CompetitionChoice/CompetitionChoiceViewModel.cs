using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;

namespace AdminView.CompetitionChoice
{
    class CompetitionChoiceViewModel : AdminViewModel<Competition>, ViewCore.Entities.ISwitchableViewModel
    {
        public CompetitionChoiceViewModel(ViewCore.Entities.IEditableCompetition currentCompteition) : base(currentCompteition)
        {
            DisplayAddCompetitionViewCmd = new RelayCommand(OnDisplayAddCompetitionView, CanDisplayAddCompetition);
            AddCompetitionViewRequested += NavToAddCompetitionView;
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            _selectedCompetition = new EditableCompetition(currentCompteition);
        }

        #region Properties
        private ObservableCollection<ViewCore.Entities.EditableCompetition> _competitions = new ObservableCollection<ViewCore.Entities.EditableCompetition>();
        public ObservableCollection<ViewCore.Entities.EditableCompetition> Competitions
        {
            get { return _competitions; }
            set { SetProperty(ref _competitions, value); }
        }

        private EditableCompetition _selectedCompetition;
        public EditableCompetition SelectedCompetition
        {
            get { return _selectedCompetition; }
            set
            {
                SetProperty(ref _selectedCompetition, value);
                if (!string.IsNullOrWhiteSpace(SelectedCompetition.Name))
                    CompetitionSelected = true;
            }
        }


        private bool _competitionSelected;
        public bool CompetitionSelected
        {
            get { return _competitionSelected; }
            set
            {
                _competitionSelected = value;
                GoToCompetitionCmd.RaiseCanExecuteChanged();
            }
        }

        #endregion

        #region Methods and Events

        private void OnViewLoaded()
        {
            //ClearDatabase();
            //FillDatabase();
            DownloadDataFromDatabase();
        }

        private async void ClearDatabase()
        {
            var repository = new CompetitionRepository(new ContextProvider());
            await repository.RemoveAllAsync();
        }

        private void FillDatabase()
        {
            var repository = new CompetitionRepository(new ContextProvider());
            AddCompetitions(repository);
        }

        private void DownloadDataFromDatabase()
        {
            var repository = new CompetitionRepository(new ContextProvider());
            DownloadCompetitions(repository);
        }

        private void OnGoToCompetition()
        {
            //ClearDataForCompetition();
            //AddDataForCompetition();
            CompetitionManagerRequested();
        }


        private bool CanGoToCompetition()
        {
            return CompetitionSelected;
        }

        private void NavToAddCompetitionView()
        {
            AddCompetition.AddCompetitionViewModel addCompetitionViewModel = new AddCompetition.AddCompetitionViewModel();
            addCompetitionViewModel.AddCompetitionRequested += OnCompetitionAddedAsync;
            addCompetitionViewModel.ShowWindow();
        }

        private async void OnCompetitionAddedAsync(object sender, EventArgs e)
        {
            var addCompetitionViewModel = sender as AddCompetition.AddCompetitionViewModel;
            EditableCompetition competitionToAdd = addCompetitionViewModel.NewCompetition;
            Competitions.Add(competitionToAdd);
            await _competitionRepository.AddAsync(competitionToAdd.DbEntity);
        }

        private void OnDisplayAddCompetitionView()
        {
            AddCompetitionViewRequested();
        }

        private bool CanDisplayAddCompetition()
        {
            return true;
        }

        public void DetachAllEvents()
        {
            Delegate[] clientList = AddCompetitionViewRequested.GetInvocationList();
            foreach (var deleg in clientList)
                AddCompetitionViewRequested -= (deleg as Action);
            clientList = CompetitonViewRequested.GetInvocationList();
            foreach (var deleg in clientList)
                CompetitonViewRequested -= (deleg as Action);
            clientList = CompetitionManagerRequested.GetInvocationList();
            foreach (var deleg in clientList)
                CompetitionManagerRequested -= (deleg as Action);
        }

        public event Action AddCompetitionViewRequested = delegate { };
        public event Action CompetitonViewRequested = delegate { };
        public event Action CompetitionManagerRequested = delegate { };

        public RelayCommand DisplayAddCompetitionViewCmd { get; private set; }
        public RelayCommand GoToCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }

        #endregion



        #region Database
        private async void DownloadCompetitions(CompetitionRepository repository)
        {
            var dbCompetitions = new List<Competition>(await repository.GetAllAsync());
            foreach (var dbCompetition in dbCompetitions)
            {
                Competitions.Add(new EditableCompetition(_currentCompetition) { DbEntity = dbCompetition });
            }
        }

        private async void AddCompetitions(CompetitionRepository repository)
        {
            await repository.AddRangeAsync(new List<Competition>()
            {
            new Competition(
                "Zawody 1", new DateTime(2017, 11, 6), null, null, null, "Poznań"),
            new Competition(
                "Zawody 2", new DateTime(2017, 11, 6), null, null, null, "Łódź"),
            new Competition(
                "Zawody 3", new DateTime(2017, 11, 6), "Opis zawodów 3", null, null, "Warszawa"),
            new Competition(
                "Zawody 4", new DateTime(2017, 12, 1), null, null, null, "Gdynia")
            });
        }


        private async void AddDataForCompetition()
        {
            var playersRepository = new PlayerRepository(new ContextProvider(), SelectedCompetition.DbEntity);
            var taskPlayers = playersRepository.AddRangeAsync(GetPlayersCollection());

            var distanceRepository = new DistanceRepository(new ContextProvider(), SelectedCompetition.DbEntity);
            var taskDistances = distanceRepository.AddRangeAsync(GetDistancesCollection());
            await Task.WhenAll(taskPlayers, taskDistances);
        }

        private IEnumerable<Player> GetPlayersCollection()
        {
            var players = new List<Player>();
            for (int i = 0; i < 200; i++)
            {
                players.Add(new Player()
                {
                    LastName = "Kierzkowski",
                    FirstName = "Jan",
                    Team = "Niezniszczalni Zgierz",
                    BirthDate = new DateTime(1994, 10, 09),
                    IsMale = true,
                    StartNumber = 18
                });
            }
            return players;
        }

        private IEnumerable<Distance> GetDistancesCollection()
        {
            var distances = new List<Distance>()
            {
                new Distance("MINI", 15, DateTime.Now, DistanceTypeEnum.DeterminedDistance),
                new Distance("GIGA", 15, DateTime.Now, DistanceTypeEnum.LimitedTime)
            };
            return distances;
        }


        private async void ClearDataForCompetition()
        {
            var playersRepository = new PlayerRepository(new ContextProvider(), SelectedCompetition.DbEntity);
            var taskPlayers = playersRepository.RemoveAllAsync();

            var distanceRepository = new DistanceRepository(new ContextProvider(), SelectedCompetition.DbEntity);
            var taskDistances = distanceRepository.RemoveAllAsync();
            await Task.WhenAll(taskPlayers, taskDistances);
        }
        #endregion

    }
}
