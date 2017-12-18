using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminView;
using MvvmHelper;
using ViewCore;
using BaseCore.DataBase;
using ViewCore.Entities;

namespace AdminView.CompetitionManager
{
    public abstract class CompetitionManagerViewModelBase: AdminViewModel, ViewCore.Entities.ISwitchableViewModel
    {
        public CompetitionManagerViewModelBase()
        {

        }

        private ObservableCollection<ITabItemViewModel> _tabItems;

        public CompetitionManagerViewModelBase(IEditableCompetition currentComptetition) : base(currentComptetition)
        {
        }

        public ObservableCollection<ITabItemViewModel> TabItems
        {
            get { return _tabItems; }
            set { SetProperty(ref _tabItems, value); }
        }

        public abstract void DetachAllEvents();

        //private void OnGoToCompetition()
        //{
            
        //}

        //private bool CanGoToCompetition()
        //{
        //    return true;
        //}

        //public RelayCommand GoToCompetitionCmd { get; private set; }
    }
}
