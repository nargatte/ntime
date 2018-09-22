using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.Interfaces
{
    public interface IStandingsComponentsFactory
    {
        IStandingsSorter CreateStandingsSorter();
        IScoreTypeAssigner CreateScoreTypeAssigner();
    }
}
