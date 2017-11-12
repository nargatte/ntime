using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.CompetitionChoice
{
    class CompetitionChoiceViewModel : BindableBase, IViewModel
    {
        public CompetitionChoiceViewModel()
        {
            DisplayAddCompetitionViewCmd = new RelayCommand(OnDisplayAddCompetitionView, CanDisplayAddCompetition);
            AddCompetitionViewRequested += NavToAddCompetitionView;
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
        }

        private void OnViewLoaded()
        {
            //var repository = new CompetitionRepository();
            //AddCompetitions(repository);
            //DownloadCompetitions(repository);
        }

        private async void AddCompetitions(BaseCore.DataBase.CompetitionRepository repository)
        {
            List<BaseCore.DataBase.Competition> _competitions = new List<BaseCore.DataBase.Competition>()
            {
            new BaseCore.DataBase.Competition(
                "Zawody 1", new DateTime(2017, 11, 6), null, null, null, "Poznań"),
            new BaseCore.DataBase.Competition(
                "Zawody 2", new DateTime(2017, 11, 6), null, null, null, "Łódź"),
            new BaseCore.DataBase.Competition(
                "Zawody 3", new DateTime(2017, 11, 6), "Opis zawodów 3", null, null, "Warszawa"),
            new BaseCore.DataBase.Competition(
                "Zawody 4", new DateTime(2017, 12, 1), null, null, null, "Gdynia")
            };
            await repository.AddRangeAsync(_competitions);
        }

        private async void DownloadCompetitions(BaseCore.DataBase.CompetitionRepository repository)
        {
            Competitions = new ObservableCollection<BaseCore.DataBase.Competition>(await repository.GetAllAsync());
        }

        #region Properties

        private ObservableCollection<BaseCore.DataBase.Competition> _competitions;
        public ObservableCollection<BaseCore.DataBase.Competition> Competitions
        {
            get { return _competitions; }
            set { SetProperty(ref _competitions, value); }
        }

        private BaseCore.DataBase.Competition _selectedCompetition;
        public BaseCore.DataBase.Competition SelectedCompetition
        {
            get { return _selectedCompetition; }
            set { SetProperty(ref _selectedCompetition, value); }
        }


        #endregion
        private void OnGoToCompetition()
        {
            CompetitionManagerRequested();
        }

        private bool CanGoToCompetition()
        {
            return true;
        }

        private void NavToAddCompetitionView()
        {
            AddCompetition.AddCompetitionViewModel addCompetitionViewModel = new AddCompetition.AddCompetitionViewModel();
            addCompetitionViewModel.AddCompetitionRequested += OnCompetitionAdded;
            addCompetitionViewModel.ShowWindow();
        }

        private void OnCompetitionAdded()
        {
            throw new NotImplementedException();
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

    }
}
