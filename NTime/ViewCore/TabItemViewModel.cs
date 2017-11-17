using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelper;

namespace ViewCore
{
    public class TabItemViewModel : AdminViewModel
    {
        public TabItemViewModel(Entities.IEditableCompetition currentCompetition) : base(currentCompetition) { }
        private string _tabTitle;
        public string TabTitle
        {
            get { return _tabTitle; }
            set { SetProperty(ref _tabTitle, value); }
        }
    }
}
