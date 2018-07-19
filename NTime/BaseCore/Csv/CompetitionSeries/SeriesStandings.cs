using BaseCore.Csv.Map;
using BaseCore.Csv.Records;
using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries
{
    public class SeriesStandings
    {
        private Dictionary<int, double> points = new Dictionary<int, double>();
        private IEnumerable<PlayerScoreRecord> scores = new List<PlayerScoreRecord>();
        private IEnumerable<PlayerScoreRecord> dnfs = new List<PlayerScoreRecord>();
        private HashSet<string> categories = new HashSet<string>();
        private Dictionary<PlayerWithPoints, PlayerWithPoints> uniquePlayers = new Dictionary<PlayerWithPoints, PlayerWithPoints>(new PlayerWithPointsEqualityComparer());
        public async Task ImportScoresFromCsv(IEnumerable<string> paths)
        {
            var competitionsScores = new List<PlayerScoreRecord[]>();
            foreach (var path in paths)
            {
                var csvImporter = new CsvImporter<PlayerScoreRecord, PlayerScoreMap>(path, ',');
                var scoresRecords = await csvImporter.GetAllRecordsAsync();
                scores = scores.Union(scoresRecords);
            }
        }

        public async Task ImportPointsTableFromCsv(string path)
        {
            var csvImporter = new CsvImporter<PlacePointsRecord, PlacePointsMap>(path, ';');
            var pointsAndPlaces = await csvImporter.GetAllRecordsAsync();
            pointsAndPlaces.ToList().ForEach(pair => points.Add(pair.Place, pair.Points));
        }

        public void CalculateResults()
        {
            FilterScores();
            GetUniqueCategories();
            GiveOutPoints();
            PrepareStandings();
        }

        private void PrepareStandings()
        {
            var categoriesStandings = categories.ToDictionary(x => x, y => new List<PlayerWithPoints>());
            uniquePlayers.ToList().ForEach(player => categoriesStandings[player.Value.AgeCategory].Add(player.Value));

            foreach (var item in categoriesStandings)
            {
                Debug.WriteLine($"Category {item.Key}");
                item.Value.ForEach(player => Debug.WriteLine(player));
                Debug.WriteLine("");
            }

        }

        private void GiveOutPoints()
        {
            scores.ToList().ForEach(player =>
            {
                var playerWithPoints = new PlayerWithPoints(player)
                {
                    Points = points[player.CategoryPlaceNumber], CompetitionsCompleted = 1
                };
                bool addedBefore = uniquePlayers.TryGetValue(playerWithPoints, out PlayerWithPoints playerFound);
                if (addedBefore)
                {
                    playerFound.Points += playerWithPoints.Points;
                    playerFound.CompetitionsCompleted += playerWithPoints.CompetitionsCompleted;
                }
                else
                    uniquePlayers.Add(playerWithPoints, playerWithPoints);
            });
        }

        private void GetUniqueCategories()
        {
            scores.ToList().ForEach(x => categories.Add(x.AgeCategory.ToUpper()));
            categories.ToList().ForEach(category => Debug.WriteLine($"Kategoria: {category}"));
        }

        private void FilterScores()
        {
            dnfs = scores.Where(x => x.FirstName.ToLower().Contains("dnf") || x.LastName.ToLower().Contains("dnf"));
            scores = scores.Where(x => x.DistancePlaceNumber > 0 && x.CategoryPlaceNumber > 0);
        }
    }
}
