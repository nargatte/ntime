using BaseCore.Csv.CompetitionSeries.Interfaces;
using BaseCore.Csv.CompetitionSeries.PlacesAndPoints;
using BaseCore.Csv.CompetitionSeries.TimeSum;
using BaseCore.Csv.Map;
using BaseCore.Csv.Records;
using BaseCore.DataBase;
using BaseCore.DataBase.Entities;
using BaseCore.DataBase.Enums;
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
        private Dictionary<int, string> _competitionsNames;
        private SeriesStandingsParameters _standingsParamters;
        private IStandingsComponentsFactory _componentsFactory;
        private IEnumerable<string> _competitionsPaths;
        private IEnumerable<PlayerScoreRecord> _scores = new List<PlayerScoreRecord>();
        private IEnumerable<PlayerScoreRecord> _dnfs = new List<PlayerScoreRecord>();
        private HashSet<string> _categories = new HashSet<string>();
        private IEnumerable<PlayerWithScores> _standingsPlayers = new List<PlayerWithScores>();
        private char _delimiter = ';';

        public SeriesStandings(Dictionary<int, string> competitionsNames, SeriesStandingsParameters standingsParameters,
            IEnumerable<string> competitionsPaths)
        {
            _competitionsNames = competitionsNames;
            _standingsParamters = standingsParameters;
            _competitionsPaths = competitionsPaths;
            _componentsFactory = ResolveStandingsComponenstFactory(standingsParameters.StandingsType);
        }

        private IStandingsComponentsFactory ResolveStandingsComponenstFactory(CompetitionStandingsType standingsType)
        {
            switch (standingsType)
            {
                case CompetitionStandingsType.PlacesAndPoints:
                    return new PlacesAndPointsComponentsFactory();
                case CompetitionStandingsType.TimeSum:
                    return new TimeSumComponentsFactory();
                default:
                    return new PlacesAndPointsComponentsFactory();
            }
        }

        public async void CalculateResults(string pointsTablePath)
        {
            _scores = await ImportScoresFromCsv(_competitionsPaths);
            var competitionPointsTable = await ImportPointsTableFromCsv(pointsTablePath);
            FilterCorrectScores();
            GetUniqueCategories();
            _standingsPlayers = AssignScores(_scores, competitionPointsTable);
            var exportableScores = PrepareStandings(_categories, _standingsPlayers);
            bool exportedCorrectly = await ExportStandingsToCsv(exportableScores, _competitionsNames.Select(pair => pair.Value));
            Debug.WriteLine($"Exported correctly: {exportedCorrectly}");
        }

        private async Task<IEnumerable<PlayerScoreRecord>> ImportScoresFromCsv(IEnumerable<string> paths)
        {
            int iter = 0;
            var competitionsScores = new List<PlayerScoreRecord[]>();
            IEnumerable<PlayerScoreRecord> downloadedScores = new List<PlayerScoreRecord>();
            foreach (var path in paths)
            {
                var csvImporter = new CsvImporter<PlayerScoreRecord, PlayerScoreMap>(path, ',');
                var scoresRecords = await csvImporter.GetAllRecordsAsync();
                foreach (var score in scoresRecords)
                    score.CompetitionId = iter;
                downloadedScores = downloadedScores.Union(scoresRecords);
                iter++;
            }
            return downloadedScores;
        }

        private async Task<Dictionary<int,double>> ImportPointsTableFromCsv(string path)
        {
            var pointsTable = new Dictionary<int, double>();
            var csvImporter = new CsvImporter<PlacePointsRecord, PlacePointsMap>(path, ';');
            var pointsAndPlaces = await csvImporter.GetAllRecordsAsync();
            pointsAndPlaces.ToList().ForEach(pair => pointsTable.Add(pair.Place, pair.Points));
            return pointsTable;
        }

        private void FilterCorrectScores(IEnumerable<PlayerScoreRecord> scores)
        {
            int limit = 20000;
            _dnfs = scores.Where(x => x.IsDNF());
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


        private IEnumerable<PlayerWithScores> AssignScores(IEnumerable<PlayerScoreRecord> scoreRecords, Dictionary<int, double> competitionPointsTable)
        {
            var scoreTypeAssigner = _componentsFactory.CreateScoreTypeAssigner();
            return scoreTypeAssigner.AssignProperScoreType(_competitionsNames, scoreRecords, competitionPointsTable);
        }

        private IEnumerable<PlayerWithScores> PrepareStandings(HashSet<string> categories, IEnumerable<PlayerWithScores> playersWithScores,
            bool verbose = true)
        {
            var categoriesStandings = categories.ToDictionary(x => x, y => new List<PlayerWithScores>());
            if (_standingsParamters.MinStartsEnabled)
                _standingsPlayers = _standingsPlayers.Where(player => player.CompetitionsStarted >= _standingsParamters.MinStartsCount);
            //if(_standingsParamters.BestScoresEnabled)
            //    foreach (var player in _standingsPlayers)
            //    {
            //        // Watch out for competitions points string. Name the Points like sum or something
            //        player.Points
            //    }
            _standingsPlayers.ToList().ForEach(player => categoriesStandings[player.AgeCategory].Add(player));

            var standingsSorter = _componentsFactory.CreateStandingsSorter();
            return standingsSorter.SortStandings(categoriesStandings, verbose);
        }

        private async Task<bool> ExportStandingsToCsv(IEnumerable<PlayerWithScores> standingPlayers, IEnumerable<string> competitionNames)
        {
            foreach (var player in standingPlayers)
            {
                player.SetPointsForCompetitions();
            }
            string exportFileName = "results.csv";
            var exporter = new CsvExporter<PlayerWithScores, PlayerWithPointsMap>(exportFileName, _delimiter);
            return await exporter.SaveAllRecordsAsync(standingPlayers, new PlayerWithPointsMap(competitionNames.ToArray(), _delimiter));
        }
    }
}
