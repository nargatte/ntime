using BaseCore.Csv.Records;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Map
{
    public class PlayerWithPointsMap : ClassMap<PlayerWithScores> 
    {
        public PlayerWithPointsMap(string[] competitionNames, char delimiter)
        {
            Map(record => record.CategoryStandingPlace).Name("M.");
            Map(record => record.FirstName).Name("imie");
            Map(record => record.LastName).Name("nazwisko");
            Map(record => record.BirthDate).Name("Rok").ConvertUsing(player => player.BirthDate.Year.ToString());
            Map(record => record.AgeCategory).Name("kat_wiek");
            Map(record => record.TotalScore).Name("punkty").ConvertUsing(player => player.TotalScore.ScoreString);
            var otherDelimiter = delimiter == ';' ? ',' : ';';
            var joinedCompetitionNames = String.Join(otherDelimiter.ToString(), competitionNames);
            Map(record => record.CompetitionsScoreExport).Name(joinedCompetitionNames);
        }

        private void CompetitionNameConverter(IWriterRow row)
        {
            row.WriteField<string>("kategoria");
        }
    }
}
