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
            Map(m => m.CategoryStandingPlace).Name("miejsce_ranking");
            Map(m => m.FirstName).Name("imie");
            Map(m => m.LastName).Name("nazwisko");
            Map(m => m.BirthDate).Name("data_urodzenia").ConvertUsing(date => date.BirthDate.Year.ToString());
            Map(m => m.AgeCategory).Name("kat_wiek");
            Map(m => m.TotalScore).Name("punkty");
            var otherDelimiter = delimiter == ';' ? ',' : ';';
            var joinedCompetitionNames = String.Join(otherDelimiter.ToString(), competitionNames);
            Map(m => m.CompetitionsScoreExport).Name(joinedCompetitionNames);
        }

        private void CompetitionNameConverter(IWriterRow row)
        {
            row.WriteField<string>("kategoria");
        }
    }
}
