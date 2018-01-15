using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;
using ViewCore.Entities;
using ViewCore.Factories;
using ViewCore.Factories.Competitions;
using ViewCore.ManagersDesktop;
using ViewCore.ManagersInterfaces;

namespace ViewCore
{
    public class CompetitionChoiceBase : BindableBase, ISwitchableViewModel
    {

        public ICompetitionManager CompetitionManager { get; set; }

        public CompetitionChoiceBase(AccountInfo user, ConnectionInfo connectionInfo)
        {

        }

        public CompetitionChoiceBase(DependencyContainer dependencyContainer)
        {
            CompetitionManager = dependencyContainer.CompetitionManagerFactory.CreateInstance(dependencyContainer.User, dependencyContainer.ConnectionInfo);
        }
        #region Properties
        private ObservableCollection<EditableCompetition> _competitions = new ObservableCollection<EditableCompetition>();
        public ObservableCollection<EditableCompetition> Competitions
        {
            get { return _competitions; }
            set { SetProperty(ref _competitions, value); }
        }

        private EditableCompetition _selectedCompetition = new EditableCompetition();
        public EditableCompetition SelectedCompetition
        {
            get { return _selectedCompetition; }
            set
            {
                SetProperty(ref _selectedCompetition, value);
                if (SelectedCompetition != null && !string.IsNullOrWhiteSpace(SelectedCompetition.Name))
                {
                    IsCompetitionSelected = true;
                }
                CompetitionSelected();
            }
        }

        private bool _isCompetitionSelected;
        public bool IsCompetitionSelected
        {
            get { return _isCompetitionSelected && SelectedCompetition!=null; }
            set { _isCompetitionSelected = value; }
        }


        #endregion

        #region Methods and Events

        public event Action CompetitionSelected = delegate { };

        public void DownloadCompetitionsFromDatabaseAndDisplay(bool OnlyWithRegistrationEnabled = false)
        {
            CompetitionManager.DownloadDataFromDatabase();
            Competitions = CompetitionManager.GetCompetitionsToDisplay();
            if (OnlyWithRegistrationEnabled)
            {
                Competitions.Where(c => c.DbEntity.SignUpEndDate > DateTime.Now);
            }
        }

        public void DetachAllEvents()
        {
            Delegate[] clientList = CompetitionSelected.GetInvocationList();
            foreach (var deleg in clientList)
                CompetitionSelected -= (deleg as Action);
        }

        #endregion
    }
}
