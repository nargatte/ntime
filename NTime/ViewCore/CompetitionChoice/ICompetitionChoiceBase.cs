using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewCore.Entities;

namespace ViewCore
{
    public interface ICompetitionChoiceBase : ITabItemViewModel, ISwitchableViewModel
    {
        CompetitionChoiceBase CompetitionData { get; }
    }
}
