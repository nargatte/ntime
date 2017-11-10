using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminView.Scores
{
    class ScoresViewModel : TabItemViewModel, IViewModel
    {
        public ScoresViewModel()
        {
            TabTitle = "Wyniki";
        }

        public void DetachAllEvents()
        {
            throw new NotImplementedException();
        }
    }
}
