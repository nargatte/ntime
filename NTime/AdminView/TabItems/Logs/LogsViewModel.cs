using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore;
using MvvmHelper;
using ViewCore.Entities;

namespace AdminView.Logs
{
    class LogsViewModel : TabItemViewModel
    {
        public LogsViewModel(IEditableCompetition currentCompetition) : base(currentCompetition)
        {
        }
    }
}
