using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;

namespace AdminView.CompetitionChoice
{
    class CompetitionChoiceViewModel : AdminViewModel, ViewCore.Entities.ISwitchableViewModel
    {
        public CompetitionChoiceViewModel(ViewCore.Entities.IEditableCompetition currentCompteition) : base(currentCompteition)
        {
            DisplayAddCompetitionViewCmd = new RelayCommand(OnDisplayAddCompetitionView, CanDisplayAddCompetition);
            AddCompetitionViewRequested += NavToAddCompetitionView;
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            ViewLoadedCmd = new RelayCommand(OnViewLoaded);
        }

        #region Database
        private async void DownloadCompetitions(BaseCore.DataBase.CompetitionRepository repository)
        {
            var donwloadedCompetitions = new ObservableCollection<BaseCore.DataBase.Competition>(await repository.GetAllAsync());
            foreach (var item in donwloadedCompetitions)
            {
                Competitions.Add(new ViewCore.Entities.EditableCompetition() { Competition = item });
            }
        }
        #endregion

        #region Properties
        private ObservableCollection<ViewCore.Entities.EditableCompetition> _competitions = new ObservableCollection<ViewCore.Entities.EditableCompetition>();
        public ObservableCollection<ViewCore.Entities.EditableCompetition> Competitions
        {
            get { return _competitions; }
            set { SetProperty(ref _competitions, value); }
        }

        private ViewCore.Entities.EditableCompetition _selectedCompetition = new ViewCore.Entities.EditableCompetition();
        public ViewCore.Entities.EditableCompetition SelectedCompetition
        {
            get { return _selectedCompetition; }
            set
            {
                SetProperty(ref _selectedCompetition, value);
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
            var repository = new CompetitionRepository(new ContextProvider());
            AddCompetitions(repository);
            DownloadCompetitions(repository);
        }

        private async void AddCompetitions(BaseCore.DataBase.CompetitionRepository repository)
        {
            await repository.AddRangeAsync(new List<BaseCore.DataBase.Competition>()
            {
            new BaseCore.DataBase.Competition(
                "Zawody 1", new DateTime(2017, 11, 6), null, null, null, "Poznań"),
            new BaseCore.DataBase.Competition(
                "Zawody 2", new DateTime(2017, 11, 6), null, null, null, "Łódź"),
            new BaseCore.DataBase.Competition(
                "Zawody 3", new DateTime(2017, 11, 6), "Opis zawodów 3", null, null, "Warszawa"),
            new BaseCore.DataBase.Competition(
                "Zawody 4", new DateTime(2017, 12, 1), null, null, null, "Gdynia")
            });
        }

        private void OnGoToCompetition()
        {
            CompetitionManagerRequested();
        }

        private bool CanGoToCompetition()
        {
            return CompetitionSelected;
        }

        private void NavToAddCompetitionView()
        {
            AddCompetition.AddCompetitionViewModel addCompetitionViewModel = new AddCompetition.AddCompetitionViewModel();
            addCompetitionViewModel.AddCompetitionRequested += OnCompetitionAdded;
            addCompetitionViewModel.ShowWindow();
        }

        private void OnCompetitionAdded(object sender, EventArgs e)
        {
            var addCompetitionViewModel = sender as AddCompetition.AddCompetitionViewModel;
            Competitions.Add(addCompetitionViewModel.NewCompetition);
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
    #endregion
}
