using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;
using ViewCore.Entities;

namespace ViewCore
{
    public class AdminViewModel : CompetitionItemBase
    {
        protected CompetitionRepository _competitionRepository { get; set; }
        //protected GateOrderItemRepository _gateOrderItemRepository { get; set; }
        //protected TimeReadRepository _timeReadRepository { get; set; }
        //protected TimeReadsLogInofRepository _timeReadsLogInofRepository { get; set; }

        public AdminViewModel(IEditableCompetition currentComptetition) : base(currentComptetition)
        { 
            ContextProvider contextProvider = new ContextProvider();
            _competitionRepository = new CompetitionRepository(contextProvider);
        }
    }
}
