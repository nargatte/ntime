using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdminView.AddCompetition;
using AdminView.CalculateStandings;
using BaseCore.Csv;
using BaseCore.Csv.CompetitionSeries;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.ManagersDesktop;

namespace AdminView.CompetitionChoice
{
    class CompetitionChoiceViewModel : BindableBase, ISwitchableViewModel, ICompetitionChoiceBase
    {
        private CompetitionChoiceBase _competitionChoiceViewModelBase;
        public CompetitionChoiceBase CompetitionData => _competitionChoiceViewModelBase;
        public string TabTitle { get; set; }

        //public CompetitionChoiceViewModel()
        //{

        //}

        public CompetitionChoiceViewModel(DependencyContainer dependencyContainer)
        {
            _competitionChoiceViewModelBase = new CompetitionChoiceBase(dependencyContainer);
            CompetitionData.CompetitionSelected += CompetitionChoiceBase_CompetitionSelected;
            DisplayAddCompetitionViewCmd = new RelayCommand(OnDisplayAddCompetitionView, CanDisplayAddCompetition);
            AddCompetitionViewRequested += NavToAddCompetitionView;
            CalculateStandingsViewRequested += NavToCalculateStandingsView;
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            CalculateStandingsCmd = new RelayCommand(OnDisplayCalculateStandingsView);
            //ViewLoadedCmd = new RelayCommand(OnViewLoaded);
            OnViewLoaded();
        }

        private void CompetitionChoiceBase_CompetitionSelected() => GoToCompetitionCmd.RaiseCanExecuteChanged();

        #region Methods and Events

        private void OnViewLoaded() => CompetitionData.DownloadCompetitionsFromDatabaseAndDisplay();

        private void OnGoToCompetition() => CompetitionManagerRequested();

        private bool CanGoToCompetition() => CompetitionData.IsCompetitionSelected;

        private void NavToAddCompetitionView()
        {
            var addCompetitionViewModel = new AddCompetitionViewModel();
            addCompetitionViewModel.AddCompetitionRequested += OnCompetitionAddedAsync;
            addCompetitionViewModel.ShowWindowDialog();
        }

        private void NavToCalculateStandingsView()
        {
            var calculateStandingsViewModel = new CalculateStandingsViewModel();
            calculateStandingsViewModel.ShowWindowDialog();
        }

        private async void OnCompetitionAddedAsync(object sender, EventArgs e)
        {
            var addCompetitionViewModel = sender as AddCompetition.AddCompetitionViewModel;
            EditableCompetition competitionToAdd = addCompetitionViewModel.NewCompetition;
            CompetitionData.Competitions.Add(competitionToAdd);
            await CompetitionData.CompetitionManager.AddAsync(competitionToAdd.DbEntity);
        }

        private void OnDisplayAddCompetitionView() => AddCompetitionViewRequested();

        private void OnDisplayCalculateStandingsView() => CalculateStandingsViewRequested();

        private bool CanDisplayAddCompetition() => true;

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
        public event Action CalculateStandingsViewRequested = delegate { };
        public event Action CompetitonViewRequested = delegate { };
        public event Action CompetitionManagerRequested = delegate { };

        public RelayCommand DisplayAddCompetitionViewCmd { get; private set; }
        public RelayCommand GoToCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand CalculateStandingsCmd { get; private set; }

        #endregion
    }
}
