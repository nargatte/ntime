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
    public class CompetitionItemBase : BindableBase
    {
        protected IEditableCompetition _currentCompetition;

        public CompetitionItemBase()
        {

        }
        public CompetitionItemBase(IEditableCompetition currentComptetition)
        {
            _currentCompetition = currentComptetition;
        }
    }
}
