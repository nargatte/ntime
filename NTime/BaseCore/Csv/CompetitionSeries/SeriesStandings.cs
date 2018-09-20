using BaseCore.Csv.Map;
using BaseCore.Csv.Records;
using BaseCore.DataBase;
using BaseCore.DataBase.Entities;
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
        private Dictionary<int, double> _points = new Dictionary<int, double>();
        private IEnumerable<PlayerScoreRecord> _scores = new List<PlayerScoreRecord>();
        private IEnumerable<PlayerScoreRecord> _dnfs = new List<PlayerScoreRecord>();
        private HashSet<string> _categories = new HashSet<string>();
        private Dictionary<PlayerWithPoints, PlayerWithPoints> _uniquePlayers = new Dictionary<PlayerWithPoints, PlayerWithPoints>(new PlayerWithPointsEqualityComparer());
        private Dictionary<int, string> _competitionsNames;
        private SeriesStandingsParameters _standingsParamters;
        private char _delimiter = ';';

        public SeriesStandings(Dictionary<int, string> competitionsNames, SeriesStandingsParameters standingsParameters)
        {
            _competitionsNames = competitionsNames;
            _standingsParamters = standingsParameters;
        }

        public async Task ImportScoresFromCsv(IEnumerable<string> paths)
        {
            int iter = 0;
            var competitionsScores = new List<PlayerScoreRecord[]>();
            foreach (var path in paths)
            {
                var csvImporter = new CsvImporter<PlayerScoreRecord, PlayerScoreMap>(path, ',');
                var scoresRecords = await csvImporter.GetAllRecordsAsync();
                foreach (var score in scoresRecords)
                    score.CompetitionId = iter;
                _scores = _scores.Union(scoresRecords);
                iter++;
            }
        }

        public async Task ImportPointsTableFromCsv(string path)
        {
            var csvImporter = new CsvImporter<PlacePointsRecord, PlacePointsMap>(path, ';');
            var pointsAndPlaces = await csvImporter.GetAllRecordsAsync();
            pointsAndPlaces.ToList().ForEach(pair => _points.Add(pair.Place, pair.Points));
        }

        public async void CalculateResults()
        {
            FilterScores();
            GetUniqueCategories();
            GiveOutPoints();
            var exportableScores = PrepareAndPrintStandings();
            bool exportedCorrectly = await ExportStandingsToCsv(exportableScores, _competitionsNames.Select(pair => pair.Value));
            Debug.WriteLine($"Exported correctly: {exportedCorrectly}");
        }

        private void FilterScores()
        {
            int limit = 20000;
            _dnfs = _scores.Where(x => x.IsDNF());
            foreach (var score in _dnfs)
            {
                score.DistancePlaceNumber = 0;
                score.CategoryPlaceNumber = 0;
            }
            _scores = _scores.Where(x => x.DistancePlaceNumber > 0 && x.CategoryPlaceNumber > 0
                && x.DistancePlaceNumber < limit && x.CategoryPlaceNumber < limit)
                .Union(_dnfs);
        }

        private void GetUniqueCategories()
        {
            _scores.ToList().ForEach(x => _categories.Add(x.AgeCategory.ToUpper()));
            _categories.ToList().ForEach(category => Debug.WriteLine($"Kategoria: {category}"));
        }


        private void GiveOutPoints()
        {
            _scores.ToList().ForEach(player =>
            {
                bool pointsPlaceExists = _points.TryGetValue(player.CategoryPlaceNumber, out double competitionPoints);
                if (pointsPlaceExists)
                {
                    var newPlayer = new PlayerWithPoints(player, _competitionsNames)
                    {
                        //Points = _points[player.CategoryPlaceNumber],
                        Points = competitionPoints,
                        CompetitionsStarted = 1,
                    };
                    bool addedBefore = _uniquePlayers.TryGetValue(newPlayer, out PlayerWithPoints playerFound);
                    var competitionPointsPair = new KeyValuePair<int, double>(player.CompetitionId, player.IsDNF() ? -1 : newPlayer.Points);
                    if (addedBefore)
                    {
                        playerFound.Points += newPlayer.Points;
                        playerFound.CompetitionsStarted += newPlayer.CompetitionsStarted;
                        playerFound.CompetitionsPoints.Add(competitionPointsPair.Key, competitionPointsPair.Value);
                    }
                    else
                    {
                        newPlayer.CompetitionsPoints.Add(competitionPointsPair.Key, competitionPointsPair.Value);
                        _uniquePlayers.Add(newPlayer, newPlayer);
                    }
                }
            });
        }

        private IEnumerable<PlayerWithPoints> PrepareAndPrintStandings()
        {
            var categoriesStandings = _categories.ToDictionary(x => x, y => new List<PlayerWithPoints>());
            _uniquePlayers.ToList().ForEach(player => categoriesStandings[player.Value.AgeCategory].Add(player.Value));
            var exportableScores = new List<PlayerWithPoints>();

            foreach (var item in categoriesStandings.OrderBy(pair => pair.Key))
            {
                int iter = 1;
                Debug.WriteLine($"Category {item.Key}");
                item.Value.OrderByDescending(player => player.Points).ToList()
                    .ForEach(player =>
                    {
                        Debug.WriteLine($"{iter,-2} {player}");
                        player.CategoryStandingPlace = iter;
                        exportableScores.Add(player);
                        iter++;
                    });
                Debug.WriteLine("");
            }
            return exportableScores;
        }

        public async Task<bool> ExportStandingsToCsv(IEnumerable<PlayerWithPoints> standingPlayers, IEnumerable<string> competitionNames)
        {
            foreach (var player in standingPlayers)
            {
                player.SetPointsForCompetitions();
            }
            string exportFileName = "results.csv";
            var exporter = new CsvExporter<PlayerWithPoints, PlayerWithPointsMap>(exportFileName, _delimiter);
            return await exporter.SaveAllRecordsAsync(standingPlayers, new PlayerWithPointsMap(competitionNames.ToArray(), _delimiter));
        }
    }
}
