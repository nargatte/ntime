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
using AdminView.AgeCategoryTemplates;
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
        public CompetitionChoiceBase CompetitionData { get; }
        public string TabTitle { get; set; }


        public CompetitionChoiceViewModel(DependencyContainer dependencyContainer)
        {
            CompetitionData = new CompetitionChoiceBase(dependencyContainer);
            DisplayAddCompetitionViewCmd = new RelayCommand(OnDisplayAddCompetitionView, CanDisplayAddCompetition);
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            CalculateStandingsCmd = new RelayCommand(OnDisplayCalculateStandingsView);
            CategoryTemplatesCmd = new RelayCommand(OnDisplayCategoryTemplatesView);

            CompetitionData.CompetitionSelected += CompetitionChoiceBase_CompetitionSelected;
            AddCompetitionViewRequested += NavToAddCompetitionView;
            CalculateStandingsViewRequested += NavToCalculateStandingsView;
            CategoryTemplatesViewRequested += NavToCategoryTemplatesView;
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

        private void NavToCategoryTemplatesView()
        {
            var categoryTemplatesViewModel = new AgeCategoryTemplatesViewModel();
            categoryTemplatesViewModel.ShowWindowDialog();
        }

        private async void OnCompetitionAddedAsync(object sender, EventArgs e)
        {
            var addCompetitionViewModel = sender as AddCompetition.AddCompetitionViewModel;
            EditableCompetition competitionToAdd = addCompetitionViewModel.NewCompetition;
            CompetitionData.Competitions.Add(competitionToAdd);
            await CompetitionData.CompetitionManager.AddAsync(competitionToAdd.DbEntity);
        }

        private void OnDisplayAddCompetitionView() => AddCompetitionViewRequested?.Invoke();

        private void OnDisplayCalculateStandingsView() => CalculateStandingsViewRequested?.Invoke();

        private void OnDisplayCategoryTemplatesView() => CategoryTemplatesViewRequested?.Invoke();

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
        public event Action CategoryTemplatesViewRequested = delegate { };
        public event Action CompetitonViewRequested = delegate { };
        public event Action CompetitionManagerRequested = delegate { };

        public RelayCommand DisplayAddCompetitionViewCmd { get; private set; }
        public RelayCommand GoToCompetitionCmd { get; private set; }
        public RelayCommand ViewLoadedCmd { get; private set; }
        public RelayCommand CalculateStandingsCmd { get; private set; }
        public RelayCommand CategoryTemplatesCmd { get; private set; }

        #endregion
    }
}
