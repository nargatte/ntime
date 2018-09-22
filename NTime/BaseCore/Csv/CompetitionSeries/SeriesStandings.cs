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
        private IEnumerable<string> _competitionsPaths;
        private IStandingsComponentsFactory _componentsFactory;
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

        public async Task<bool> CalculateResults(string pointsTablePath)
        {
            var downloadedScores = await ImportScoresFromCsv(_competitionsPaths);
            var competitionPointsTable = await ImportPointsTableFromCsv(pointsTablePath);
            (var scores, var dnfs) = FilterCorrectScores(downloadedScores);
            var categories = GetUniqueCategories(scores);
            var playersReadyForStandings= AssignScores(scores, competitionPointsTable);
            var exportableScores = PrepareStandings(categories, playersReadyForStandings);
            bool exportedCorrectly = await ExportStandingsToCsv(exportableScores, _competitionsNames.Select(pair => pair.Value));
            Debug.WriteLine($"Exported correctly: {exportedCorrectly}");
            return exportedCorrectly;
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

        private async Task<Dictionary<int, double>> ImportPointsTableFromCsv(string path)
        {
            var pointsTable = new Dictionary<int, double>();
            var csvImporter = new CsvImporter<PlacePointsRecord, PlacePointsMap>(path, ';');
            var pointsAndPlaces = await csvImporter.GetAllRecordsAsync();
            pointsAndPlaces.ToList().ForEach(pair => pointsTable.Add(pair.Place, pair.Points));
            return pointsTable;
        }

        private (IEnumerable<PlayerScoreRecord> filteredScores, IEnumerable<PlayerScoreRecord> dnfs) FilterCorrectScores(
            IEnumerable<PlayerScoreRecord> scores)
        {
            int limit = 20000;
            var dnfs = scores.Where(x => x.IsDNF());
            foreach (var score in dnfs)
            {
                score.DistancePlaceNumber = 0;
                score.CategoryPlaceNumber = 0;
            }
            var filteredScores = scores.Where(x => x.DistancePlaceNumber > 0 && x.CategoryPlaceNumber > 0
                && x.DistancePlaceNumber < limit && x.CategoryPlaceNumber < limit)
                .Union(dnfs);
            return (filteredScores, dnfs);
        }

        private HashSet<string> GetUniqueCategories(IEnumerable<PlayerScoreRecord> scores)
        {
            var categories = new HashSet<string>();
            scores.ToList().ForEach(x => categories.Add(x.AgeCategory.ToUpper()));
            categories.ToList().ForEach(category => Debug.WriteLine($"Kategoria: {category}"));
            return categories;
        }


        private IEnumerable<PlayerWithScores> AssignScores(IEnumerable<PlayerScoreRecord> scoreRecords, Dictionary<int, double> competitionPointsTable)
        {
            var scoreTypeAssigner = _componentsFactory.CreateScoreTypeAssigner();
            return scoreTypeAssigner.AssignProperScoreType(_competitionsNames, scoreRecords, competitionPointsTable);
        }

        private IEnumerable<PlayerWithScores> PrepareStandings(HashSet<string> categories, IEnumerable<PlayerWithScores> playersReadyForStandings,
            bool verbose = true)
        {
            var categoriesStandings = categories.ToDictionary(x => x, y => new List<PlayerWithScores>());
            if (_standingsParamters.MinStartsEnabled)
                playersReadyForStandings = playersReadyForStandings.Where(player => player.CompetitionsStarted >= _standingsParamters.MinStartsCount);
            //if(_standingsParamters.BestScoresEnabled)
            //    foreach (var player in _standingsPlayers)
            //    {
            //        // Watch out for competitions points string. Name the Points like sum or something
            //        player.Points
            //    }
            playersReadyForStandings.ToList().ForEach(player => categoriesStandings[player.AgeCategory].Add(player));

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
