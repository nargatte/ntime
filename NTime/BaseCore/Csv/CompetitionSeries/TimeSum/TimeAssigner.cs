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
    public class TimeAssigner : IScoreTypeAssigner
    {
        public IEnumerable<PlayerWithScores> AssignProperScoreType(SeriesStandingsParameters standingsParameters,
            Dictionary<int, string> competitionsNames, IEnumerable<PlayerScoreRecord> scoreRecords,
            Dictionary<int, double> competitionPointsTable)
        {
            throw new NotImplementedException();
        }
    }
}
