using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.AddCompetition
{
    class AddCompetitionViewModel : BindableBase, IViewModel
    {
        AddCompetitionView view;
        public AddCompetitionViewModel()
        {
            AddCompetitionCmd = new RelayCommand(OnAddCompetition, CanAddCompetition);
            CancelAddingCmd = new RelayCommand(OnCancelAdding);
        }

        private Entities.EditableCompetition _newCompetition;
        public Entities.EditableCompetition NewCompetition
        {
            get { return _newCompetition; }
            set { SetProperty(ref _newCompetition, value); }
        }

        public void ShowWindow()
        {
            view = new AddCompetitionView() { DataContext = this };
            view.ShowDialog();
        }

        private void OnAddCompetition()
        {
            AddCompetitionRequested(this, EventArgs.Empty);
            view.Close();
        }

        private bool CanAddCompetition()
        {
            return false;
        }

        private void OnCancelAdding()
        {
            view.Close();
        }

        public void DetachAllEvents()
        {
            Delegate[] clientList = AddCompetitionRequested.GetInvocationList();
            foreach (var deleg in clientList)
                AddCompetitionRequested -= (deleg as EventHandler);
            clientList = CancelAddingRequested.GetInvocationList();
            foreach (var deleg in clientList)
                CancelAddingRequested -= (deleg as Action);
        }

        private ObservableCollection<BaseCore.DataBase.Competition> _competition;
        public ObservableCollection<BaseCore.DataBase.Competition> Competition
        {
            get { return _competition; }
            set { SetProperty(ref _competition, value); }
        }

        public RelayCommand AddCompetitionCmd { get; private set; }
        public RelayCommand CancelAddingCmd { get; private set; }
        public event EventHandler AddCompetitionRequested = delegate { };
        public event Action CancelAddingRequested = delegate { };
    }
}
