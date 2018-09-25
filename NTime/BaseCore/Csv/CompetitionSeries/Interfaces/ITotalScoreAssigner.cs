using BaseCore.Csv.Records;
using BaseCore.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.Interfaces
{
    public interface ITotalScoreAssigner
    {
        HashSet<PlayerWithScores> CalculateAndAssignTotalScore(IStandingsComponentsFactory componentsFactory,
            SeriesStandingsParameters standingsParameters, HashSet<PlayerWithScores> uniquePlayers);
    }
}
