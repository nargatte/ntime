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
        public IEnumerable<PlayerWithScores> AssignProperScoreType(IStandingsComponentsFactory componentsFactory,
            SeriesStandingsParameters standingsParameters, Dictionary<int, string> competitionsNames, IEnumerable<PlayerScoreRecord> scoreRecords,
            Dictionary<int, double> competitionPointsTable)
        {
            var uniquePlayers = new HashSet<PlayerWithScores>(new PlayerWithPointsEqualityComparer());
            scoreRecords.ToList().ForEach(player =>
            {
                var time = player.RaceTime > TimeSpan.Zero ? player.RaceTime : TimeSpan.Zero;
                var newPlayer = new PlayerWithScores(player, competitionsNames)
                {
                    ExactCompetitionsStarted = 1,
                };
                bool addedBefore = uniquePlayers.TryGetValue(newPlayer, out PlayerWithScores foundPlayer);
                var competitionTimePair = new KeyValuePair<int, IPlayerScore>(player.CompetitionId,
                    new TimeScore(time, player.IsDNF(), startedInCompetition: true));
                if (addedBefore)
                {
                    foundPlayer.ExactCompetitionsStarted += newPlayer.ExactCompetitionsStarted;
                    foundPlayer.CompetitionsScores.Add(competitionTimePair.Key, competitionTimePair.Value);
                }
                else
                {
                    newPlayer.CompetitionsScores.Add(competitionTimePair.Key, competitionTimePair.Value);
                    uniquePlayers.Add(newPlayer);
                }
            });
            var totalScoreAssigner = componentsFactory.CreateTotalScoreAssigner();
            uniquePlayers = totalScoreAssigner.CalculateTotalScore(componentsFactory, standingsParameters, uniquePlayers);
            return uniquePlayers;
        }
    }
}
