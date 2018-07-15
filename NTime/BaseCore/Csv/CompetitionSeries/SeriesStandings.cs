using BaseCore.Csv.Map;
using BaseCore.Csv.Records;
using BaseCore.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.CompetitionSeries
{
    public class SeriesStandings
    {
        public async void ImportScoresFromCsv(IEnumerable<string> paths)
        {
            foreach (var path in paths)
            {
                var csvImporter = new CsvImporter<PlayerScoreRecord, PlayerScoreMap>(path, ',');
                var scoresRecords = await csvImporter.GetAllRecordsAsync();
            }
        }
    }
}
