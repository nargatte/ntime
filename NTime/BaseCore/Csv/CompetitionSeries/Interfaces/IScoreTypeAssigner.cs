using BaseCore.Csv.Records;
using BaseCore.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.Interfaces
{
    public interface IScoreTypeAssigner
    {
        IEnumerable<PlayerWithScores> AssignProperScoreType(IStandingsComponentsFactory componentsFactory, 
            SeriesStandingsParameters standingsParameters, Dictionary<int, string> competitionsNames, IEnumerable<PlayerScoreRecord> scoreRecords,
            Dictionary<int, double> competitionPointsTable);
    }
}
