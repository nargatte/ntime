using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseCore.DataBase;
using MvvmHelper;

namespace ViewCore
{
    public class AdminViewModel : EditableBaseClass<Competition>
    {
        protected CompetitionRepository _competitionRepository { get; set; }
        //protected GateOrderItemRepository _gateOrderItemRepository { get; set; }
        //protected TimeReadRepository _timeReadRepository { get; set; }
        //protected TimeReadsLogInofRepository _timeReadsLogInofRepository { get; set; }

        protected AdminViewModel(ViewCore.Entities.IEditableCompetition currentComptetition)
        {
            _currentCompetition = currentComptetition;
            ContextProvider contextProvider = new ContextProvider();
            _competitionRepository = new CompetitionRepository(contextProvider);
        }



        protected AdminViewModel() { }
    }
}
