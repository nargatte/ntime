using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace ViewCore
{
    public class AdminViewModel : BindableBase
    {
        protected ViewCore.Entities.IEditableCompetition _currentCompetition;

        protected AdminViewModel(ViewCore.Entities.IEditableCompetition currentComptetition)
        {
            _currentCompetition = currentComptetition;
        }
        protected AdminViewModel() { }
    }
}
