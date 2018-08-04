using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DesktopClientView.TabManager;
using MvvmHelper;
using ViewCore;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.Factories.AgeCategories;
using ViewCore.Factories.Competitions;
using ViewCore.Factories.Distances;
using ViewCore.Factories.Subcategories;
using ViewCore.Factories.PlayerAccounts;
using ViewCore.Factories.Players;

namespace DesktopClientView
{
    internal class MainWindowViewModel : BindableBase
    {
        private ISwitchableViewModel _currentViewModel;
        private DependencyContainer dependencyContainer;

        public MainWindowViewModel()
        {
            PrepareDependencies();
            CurrentViewModel = new TabManagerViewModel(dependencyContainer);
        }

        private void PrepareDependencies()
        {
            var user = new AccountInfo();
            //var connectionInfo = new ConnectionInfo() { ServerURL = "http://localhost:58445" };
            //var connectionInfo = new ConnectionInfo() { ServerURL = "http://projektnet.mini.pw.edu.pl/NTime" };
            var connectionInfo = new ConnectionInfo() { ServerURL = "https://www.testing.time2win.aspnet.pl" };

            var playerManagerFactory = new PlayerManagerFactoryHttp();
            var distanceManagerFactory = new DistanceManagerFactoryHttp();
            var subcategoryManagerFactory = new SubcategoryManagerFactoryHttp();
            var ageCategoryManagerFactory = new AgeCategoryManagerFactoryHttp();
            var competitionManagerFactory = new CompetitionManagerFactoryHttp();
            var playerAccountManagerFactory = new PlayerAccountManagerFactoryHttp();

            //var playerManagerFactory = new PlayerManagerFactoryDesktop();
            //var distanceManagerFactory = new DistanceManagerFactoryDesktop();
            //var extraPlayerInfoManagerFactory = new ExtraPlayerInfoManagerFactoryDesktop();
            //var ageCategoryManagerFactory = new AgeCategoryManagerFactoryDesktop();
            //var competitionManagerFactory = new CompetitionManagerFactoryDesktop();
            //var playerAccountManagerFactory = new PlayerAccountManagerFactoryHttp();

            dependencyContainer = new DependencyContainer(ageCategoryManagerFactory, competitionManagerFactory, distanceManagerFactory,
                                                            subcategoryManagerFactory, playerManagerFactory, playerAccountManagerFactory,
                                                            user, connectionInfo);
        }

        public ISwitchableViewModel CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }
    }
}
