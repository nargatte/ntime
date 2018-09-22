using BaseCore.Csv.CompetitionSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.TimeSum
{
    public class TimeSumComponentsFactory : IStandingsComponentsFactory
    {
        public IScoreTypeAssigner CreateScoreTypeAssigner()
        {
            throw new NotImplementedException();
        }

        public IStandingsSorter CreateStandingsSorter()
        {
            throw new NotImplementedException();
        }
    }
}
