﻿using BaseCore.Csv.Map;
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
        private IEnumerable<PlayerScoreRecord> scores = new List<PlayerScoreRecord>();
        private IEnumerable<PlayerScoreRecord> dnfs = new List<PlayerScoreRecord>();
        private HashSet<string> categories = new HashSet<string>();
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
        }

        public void CalculateResults()
        {
            FilterScores();
            GetUniqueCategories();
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
