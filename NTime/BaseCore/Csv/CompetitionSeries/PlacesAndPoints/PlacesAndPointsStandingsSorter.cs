using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.Records;
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
        public List<PlayerWithScores> SortStandings(Dictionary<string, List<PlayerWithScores>> categoriesStandings, bool verbose)
        {
            var exportableScores = new List<PlayerWithScores>();

            foreach (var item in categoriesStandings.OrderBy(pair => pair.Key))
            {
                int iter = 1;
                if (verbose)
                    Debug.WriteLine($"Category {item.Key}");
                item.Value.OrderByDescending(player => player.Points).ToList()
                    .ForEach(player =>
                    {
                        if (verbose)
                            Debug.WriteLine($"{iter,-2} {player}");
                        player.CategoryStandingPlace = iter;
                        exportableScores.Add(player);
                        iter++;
                    });
                if (verbose)
                    Debug.WriteLine("");
            }
            return exportableScores;
        }
    }
}
