using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;


namespace ViewCore
{
    public class TabItemViewModel : AdminViewModel, ITabItemViewModel
    {
        protected AgeCategoryRepository _ageCategoryRepository { get; set; }
        protected DistanceRepository _distanceRepository { get; set; }
        protected ExtraPlayerInfoRepository _extraPlayerInfoRepository { get; set; }
        protected GateRepository _gateRepository { get; set; }
        protected PlayerRepository _playerRepository { get; set; }

        public TabItemViewModel()
        {

        }

        public TabItemViewModel(Entities.IEditableCompetition currentCompetition) : base(currentCompetition)
        {
            CreateRepositoriesForCurrentCompetition();
        }
        private string _tabTitle;
        public string TabTitle
        {
            get { return _tabTitle; }
            set { SetProperty(ref _tabTitle, value); }
        }

        private void CreateRepositoriesForCurrentCompetition()
        {
            ContextProvider contextProvider = new ContextProvider();
            _ageCategoryRepository = new AgeCategoryRepository(contextProvider, _currentCompetition.DbEntity);
            _distanceRepository = new DistanceRepository(contextProvider, _currentCompetition.DbEntity);
            _extraPlayerInfoRepository = new ExtraPlayerInfoRepository(contextProvider, _currentCompetition.DbEntity);
            _gateRepository = new GateRepository(contextProvider, _currentCompetition.DbEntity);
            _playerRepository = new PlayerRepository(contextProvider, _currentCompetition.DbEntity);
            //_gateOrderItemRepository = new GateOrderItemRepository(contextProvider, );
            //_timeReadRepository = new TimeReadRepository()
            //_timeReadsLogInofRepository = new TimeReadsLogInofRepository()

        }

    }
}
