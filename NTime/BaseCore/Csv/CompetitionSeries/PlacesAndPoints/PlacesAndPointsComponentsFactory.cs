using BaseCore.Csv.CompetitionSeries.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.PlacesAndPoints
{
    public class PlacesAndPointsComponentsFactory : IStandingsComponentsFactory
    {
        public IPlayerScore CreateDefaultPlayerScore()
        {
            return new PointsScore(0, false, true);
        }

        public IScoreFilter CreateScoreFilter()
        {
            return new PlacesAndPointsScoreFilter();
        }

        public IScoreTypeAssigner CreateScoreTypeAssigner()
        {
            return new PointsAssigner();
        }

        public IStandingsSorter CreateStandingsSorter()
        {
            return new PlacesAndPointsStandingsSorter();
        }

        public ITotalScoreAssigner CreateTotalScoreAssigner()
        {
            return new TotalPointsAssigner();
        }
    }
}
