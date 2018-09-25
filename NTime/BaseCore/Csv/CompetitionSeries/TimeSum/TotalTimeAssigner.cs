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
    public class TotalTimeAssigner : ITotalScoreAssigner
    {
        public HashSet<PlayerWithScores> CalculateTotalScore(IStandingsComponentsFactory componentsFactory, SeriesStandingsParameters standingsParameters, HashSet<PlayerWithScores> uniquePlayers)
        {
            foreach (var player in uniquePlayers)
            {
                var allScores = player.CompetitionsScores.Select(pair => pair.Value);
                if (standingsParameters.BestScoresEnabled)
                {
                    allScores = allScores.Where(score => score.NumberValue > 0)
                                        .OrderBy(score => score.NumberValue)
                                        .Take(standingsParameters.BestCompetitionsCount);
                }
                var totalScore = componentsFactory.CreateDefaultPlayerScore();
                allScores.ToList().ForEach(score => totalScore.AddValue(score));
                player.TotalScore = totalScore;
            }
            return uniquePlayers;
        }

        public HashSet<PlayerWithScores> CalculateApproximateCompetitonsStarted(SeriesStandingsParameters standingsParameters,
            HashSet<PlayerWithScores> uniquePlayers)
        {
            foreach (var player in uniquePlayers)
            {
                player.ApproximateCompetitionsStarted = standingsParameters.MinStartsEnabled ?
                    Math.Min(player.ExactCompetitionsStarted, standingsParameters.MinStartsCount) :
                    player.ExactCompetitionsStarted;
            }
            return uniquePlayers;
        }
    }
}
