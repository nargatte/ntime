using System;
using System.Collections.Generic;
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
        }

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

    }
}
