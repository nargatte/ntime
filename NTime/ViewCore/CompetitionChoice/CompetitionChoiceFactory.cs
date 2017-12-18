using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewCore
{
    public class CompetitionChoiceFactory
    {
        public static CompetitionChoiceBase NewCompetitionChoiceViewModelBase()
        {
            return new CompetitionChoiceBase();
        }
    }
}
