using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using BaseCore.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.PlacesAndPoints
{
    public class PlacesAndPointsStandingsSorter : IStandingsSorter
    {
        public IEnumerable<PlayerWithScores> SortStandings(SeriesStandingsParameters standingsParameters,
            IEnumerable<PlayerWithScores> allCategoryPlayers)
        {
            if (standingsParameters.SortByStartsCountFirst)
                return allCategoryPlayers.OrderBy(player => player.CompetitionsStarted).ThenBy(player => player.TotalScore.NumberValue);
            else
                return allCategoryPlayers.OrderBy(player => player.TotalScore.NumberValue);
        }
    }
}
