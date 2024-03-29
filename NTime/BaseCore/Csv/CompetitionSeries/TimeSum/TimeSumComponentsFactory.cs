﻿using BaseCore.Csv.CompetitionSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.TimeSum
{
    public class TimeSumComponentsFactory : IStandingsComponentsFactory
    {
        public IPlayerScore CreateDefaultPlayerScore()
        {
            return new TimeScore(TimeSpan.FromMilliseconds(0), false, true);
        }

        public IScoreFilter CreateScoreFilter()
        {
            return new TimeSumScoreFilter();
        }

        public IScoreTypeAssigner CreateScoreTypeAssigner()
        {
            return new TimeAssigner();
        }

        public IStandingsSorter CreateStandingsSorter()
        {
            return new TimeSumStandingsSorter();
        }

        public ITotalScoreAssigner CreateTotalScoreAssigner()
        {
            return new TotalTimeAssigner();
        }
    }
}
