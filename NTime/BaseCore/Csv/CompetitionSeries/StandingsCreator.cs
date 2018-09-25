using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
using BaseCore.DataBase.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries
{
    public class StandingsCreator
    {
        public IEnumerable<PlayerWithScores> CreateStandings(IStandingsComponentsFactory componentsFactory, SeriesStandingsParameters standingsParameters,
            HashSet<string> categories, IEnumerable<PlayerWithScores> playersReadyForStandings, bool verbose = true)
        {
            var categoriesStandings = categories.ToDictionary(x => x, y => new List<PlayerWithScores>());
            if (standingsParameters.StrictMinStartsEnabled)
                playersReadyForStandings = playersReadyForStandings.Where(player => player.ExactCompetitionsStarted >= standingsParameters.StrictMinStartsCount);
            playersReadyForStandings.ToList().ForEach(player => categoriesStandings[player.AgeCategory].Add(player));

            var standingsSorter = componentsFactory.CreateStandingsSorter();
            var exportableScores = new List<PlayerWithScores>();
            foreach (var item in categoriesStandings.OrderBy(pair => pair.Key))
            {
                if (verbose)
                    Debug.WriteLine($"Category {item.Key}");
                IEnumerable<PlayerWithScores> allCategoryPlayers = item.Value;
                allCategoryPlayers = standingsSorter.SortStandings(standingsParameters, allCategoryPlayers);
                int iter = 0;
                double previousValue = double.MinValue;
                int exAequoCount = 1;
                allCategoryPlayers.ToList().ForEach(player =>
                {
                    if (player.TotalScore.NumberValue == previousValue)
                        exAequoCount++;
                    else
                    {
                        iter += exAequoCount;
                        exAequoCount = 1;
                    }
                    previousValue = player.TotalScore.NumberValue;
                    player.CategoryStandingPlace = iter;
                    if (verbose)
                        Debug.WriteLine($"{iter,-2} {player}");
                    exportableScores.Add(player);
                });
                if (verbose)
                    Debug.WriteLine("");
            }
            return exportableScores;
        }
    }
}
