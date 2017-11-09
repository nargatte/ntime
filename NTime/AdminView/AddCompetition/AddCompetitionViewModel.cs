using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;

namespace AdminView.AddCompetition
{
    class AddCompetitionViewModel : BindableBase, IViewModel
    {
        AddCompetitionView view;
        public AddCompetitionViewModel()
        {
            _competition = new ObservableCollection<BaseCore.DataBase.Competition>();
            Competition.Add(new Competition
            {
                Id = 78,
                Name = "Comp",
                City = "Poznań",
                Description = "W poznaniu",
                EventDate = new DateTime(2017, 12, 3),
                CompetitionTypeId = (int)CompetitionTypeEnum.DeterminedDistance
            });
            AddCompetitionCmd = new RelayCommand(OnAddCompetition, CanAddCompetition);
            CancelAddingCmd = new RelayCommand(OnCancelAdding);
        }


        public void ShowWindow()
        {
            view = new AddCompetitionView() { DataContext = this };
            view.ShowDialog();
        }

        private void OnAddCompetition()
        {
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
                AddCompetitionRequested -= (deleg as Action);
            clientList = CancelAddingRequested.GetInvocationList();
            foreach (var deleg in clientList)
                CancelAddingRequested -= (deleg as Action);
        }

        private ObservableCollection<Competition> _competition;
        public ObservableCollection<Competition> Competition
        {
            get { return _competition; }
            set { SetProperty(ref _competition, value); }
        }

        public RelayCommand AddCompetitionCmd { get; private set; }
        public RelayCommand CancelAddingCmd { get; private set; }
        public event Action AddCompetitionRequested = delegate { };
        public event Action CancelAddingRequested = delegate { };
    }
}
