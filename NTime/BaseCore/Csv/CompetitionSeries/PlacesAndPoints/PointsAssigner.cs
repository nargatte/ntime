using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries.PlacesAndPoints
{
    public class PointsAssigner : IScoreTypeAssigner
    {
        public IEnumerable<PlayerWithScores> AssignProperScoreType(Dictionary<int, string> competitionsNames,
            IEnumerable<PlayerScoreRecord> scoreRecords, Dictionary<int, double> competitionPointsTable)
        {
            var uniquePlayers = new HashSet<PlayerWithScores>(new PlayerWithPointsEqualityComparer());
            scoreRecords.ToList().ForEach(player =>
            {
                bool pointsPlaceExists = competitionPointsTable.TryGetValue(player.CategoryPlaceNumber, out double competitionPoints);
                if (pointsPlaceExists)
                {
                    var newPlayer = new PlayerWithScores(player, competitionsNames)
                    {
                        //Points = _points[player.CategoryPlaceNumber],
                        Points = competitionPoints,
                        CompetitionsStarted = 1,
                    };
                    bool addedBefore = uniquePlayers.TryGetValue(newPlayer, out PlayerWithScores foundPlayer);
                    var competitionPointsPair = new KeyValuePair<int, double>(player.CompetitionId, player.IsDNF() ? -1 : newPlayer.Points);
                    if (addedBefore)
                    {
                        foundPlayer.Points += newPlayer.Points;
                        foundPlayer.CompetitionsStarted += newPlayer.CompetitionsStarted;
                        foundPlayer.CompetitionsPoints.Add(competitionPointsPair.Key, competitionPointsPair.Value);
                    }
                    else
                    {
                        newPlayer.CompetitionsPoints.Add(competitionPointsPair.Key, competitionPointsPair.Value);
                        uniquePlayers.Add(newPlayer);
                    }
                }
            });
            return uniquePlayers;
        }
    }
}
