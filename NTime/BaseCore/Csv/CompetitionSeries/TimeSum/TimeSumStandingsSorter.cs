using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using BaseCore.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.TimeSum
{
    public class TimeSumStandingsSorter : IStandingsSorter
    {
        public IEnumerable<PlayerWithScores> SortStandings(SeriesStandingsParameters standingsParameters,
            IEnumerable<PlayerWithScores> allCategoryPlayers)
        {
            throw new NotImplementedException();
        }
    }
}
