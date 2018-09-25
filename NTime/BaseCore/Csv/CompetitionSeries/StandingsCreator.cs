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
            categoriesStandings = AssignCategoryStandingPlaces(standingsParameters, categoriesStandings, standingsSorter);
            if (verbose)
                PrintStandingsInDebug(categoriesStandings);
            var exportableScores = PrepareExportableScores(standingsParameters, categoriesStandings);
            return exportableScores;
        }

        private Dictionary<string, List<PlayerWithScores>> AssignCategoryStandingPlaces(SeriesStandingsParameters standingsParameters, Dictionary<string, List<PlayerWithScores>> categoriesStandings, IStandingsSorter standingsSorter)
        {
            var sortedCategoriesStandings = new Dictionary<string, List<PlayerWithScores>>();
            foreach (var pair in categoriesStandings.OrderBy(pair => pair.Key))
            {
                IEnumerable<PlayerWithScores> allCategoryPlayers = new List<PlayerWithScores>(pair.Value);
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
                });
                sortedCategoriesStandings.Add(pair.Key, allCategoryPlayers.ToList());
            }
            return sortedCategoriesStandings;
        }

        private void PrintStandingsInDebug(Dictionary<string, List<PlayerWithScores>> categoriesStandings)
        {
            foreach (var pair in categoriesStandings.OrderBy(pair => pair.Key))
            {
                Debug.WriteLine($"Category {pair.Key}");
                IEnumerable<PlayerWithScores> allCategoryPlayers = pair.Value;
                foreach (var player in allCategoryPlayers) { Debug.WriteLine($"{player.CategoryStandingPlace,-2} {player}"); }
                Debug.WriteLine("");
            }
        }

        private IEnumerable<PlayerWithScores> PrepareExportableScores(SeriesStandingsParameters standingsParameters, Dictionary<string, List<PlayerWithScores>> categoriesStandings)
        {
            var exportableScores = new List<PlayerWithScores>();
            foreach (var pair in categoriesStandings.OrderBy(pair => pair.Key))
            {
                IEnumerable<PlayerWithScores> allCategoryPlayers = pair.Value;
                if(standingsParameters.PrintBestOnly)
                {
                    allCategoryPlayers = allCategoryPlayers.Take(standingsParameters.PrintBestCount);
                }
                foreach (var player in allCategoryPlayers) { exportableScores.Add(player); }
            }
            return exportableScores;
        }
    }
}
