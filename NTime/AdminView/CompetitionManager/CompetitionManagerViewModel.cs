using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView;
using AdminView.Settings;
using AdminView.Players;
using AdminView.Scores;
using AdminView.Distances;
using AdminView.Categories;
using AdminView.Logs;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Distances;
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.Players;
using ViewCore.Entities;
using ViewCore.Factories;

namespace AdminView.CompetitionManager
{
    class CompetitionManagerViewModel : CompetitionManagerViewModelBase
    {
        protected IPlayerManagerFactory _playerManagerFactory = new PlayerManagerFactoryDesktop();
        protected IDistanceManagerFactory _distanceManagerFactory = new DistanceManagerFactoryDesktop();
        protected ISubcategoryManagerFactory _subcategoryManagerFactory = new SubcategoryManagerFactoryDesktop();
        protected IAgeCategoryManagerFactory _ageCategoryManagerFactory = new AgeCategoryManagerFactoryDesktop();

        //public CompetitionManagerViewModel(ViewCore.Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        //{
        //    GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
        //    TabItems = new ObservableCollection<ITabItemViewModel>()
        //    {
        //        new SettingsViewModel(_currentCompetition),
        //        new PlayersViewModel(_currentCompetition, _playerManagerFactory, _distanceManagerFactory, _subcategoryManagerFactory, _ageCategoryManagerFactory),
        //        new DistancesViewModel(_currentCompetition),
        //        new CategoriesViewModel(_currentCompetition),
        //        new LogsViewModel(_currentCompetition),
        //        new ScoresViewModel(_currentCompetition, _playerManagerFactory, _distanceManagerFactory, _subcategoryManagerFactory, _ageCategoryManagerFactory)
        //    };
        //}

        public CompetitionManagerViewModel(IEditableCompetition currentCompetition, DependencyContainer dependencyContainer) : base(currentCompetition)
        {
            GoToCompetitionCmd = new RelayCommand(OnGoToCompetition, CanGoToCompetition);
            var settingViewModel = new SettingsViewModel(_currentCompetition);
            settingViewModel.CompetitionRemoved += SettingViewModel_CompetitionRemoved;
            TabItems = new ObservableCollection<ITabItemViewModel>()
            {
                settingViewModel,
                new PlayersViewModel(_currentCompetition, dependencyContainer),
                new DistancesViewModel(_currentCompetition),
                new CategoriesViewModel(_currentCompetition),
                new LogsViewModel(_currentCompetition),
                new ScoresViewModel(_currentCompetition, dependencyContainer)
            };
        }

        private void SettingViewModel_CompetitionRemoved()
        {
            CompetitionRemoved?.Invoke();
        }

        private void OnGoToCompetition()
        {
            // TODO: Clean it up
        }

        private bool CanGoToCompetition()
        {
            return true;
        }

        public override void DetachAllEvents()
        {

        }

        public RelayCommand GoToCompetitionCmd { get; private set; }

        public event Action CompetitionRemoved = delegate { };
    }
}
