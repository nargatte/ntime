using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Scores
{
    class ScoresViewModel : BindableBase, IViewModel
    {
        public ScoresViewModel()
        {
            TabTitle = "Wyniki";
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }

        private string _tabTitle;
        public string TabTitle
        {
            get { return _tabTitle; }
            set { SetProperty(ref _tabTitle, value); }
        }
    }
}
