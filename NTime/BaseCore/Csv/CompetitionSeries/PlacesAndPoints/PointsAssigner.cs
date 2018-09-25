using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using BaseCore.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.PlacesAndPoints
{
    public class PointsAssigner : IScoreTypeAssigner
    {
        public IEnumerable<PlayerWithScores> AssignProperScoreType(IStandingsComponentsFactory componentsFactory,
            SeriesStandingsParameters standingsParameters, Dictionary<int, string> competitionsNames, IEnumerable<PlayerScoreRecord> scoreRecords,
            Dictionary<int, double> competitionPointsTable)
        {
            var uniquePlayers = new HashSet<PlayerWithScores>(new PlayerWithPointsEqualityComparer());
            scoreRecords.ToList().ForEach(player =>
            {
                bool pointsPlaceExists = competitionPointsTable.TryGetValue(player.CategoryPlaceNumber, out double competitionPoints);
                if (pointsPlaceExists)
                {
                    var newPlayer = new PlayerWithScores(player, competitionsNames)
                    {
                        CompetitionsStarted = 1,
                    };
                    bool addedBefore = uniquePlayers.TryGetValue(newPlayer, out PlayerWithScores foundPlayer);
                    var competitionPointsPair = new KeyValuePair<int, IPlayerScore>(player.CompetitionId,
                        new PointsScore(competitionPoints, player.IsDNF(), startedInCompetition: true));
                    if (addedBefore)
                    {
                        foundPlayer.CompetitionsStarted += newPlayer.CompetitionsStarted;
                        foundPlayer.CompetitionsScores.Add(competitionPointsPair.Key, competitionPointsPair.Value);
                    }
                    else
                    {
                        newPlayer.CompetitionsScores.Add(competitionPointsPair.Key, competitionPointsPair.Value);
                        uniquePlayers.Add(newPlayer);
                    }
                }
            });
            var totalScoreAssigner = componentsFactory.CreateTotalScoreAssigner();
            uniquePlayers = totalScoreAssigner.CalculateAndAssignTotalScore(componentsFactory, standingsParameters, uniquePlayers);
            return uniquePlayers;
        }

        
    }
}
