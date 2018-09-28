using BaseCore.Csv.Helpers;
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
    class PlayerScoreMap : ClassMap<PlayerScoreRecord>
    {
        public PlayerScoreMap()
        {
            Map(record => record.LastName).Name("nazwisko");
            Map(record => record.FirstName).Name("imie");
            Map(record => record.BirthDate).ConvertUsing(DateBirthConverter);
            Map(record => record.AgeCategory).ConvertUsing(CategoryConverter);
            Map(record => record.DistancePlaceNumber).ConvertUsing(DistancePlaceConverter);
            Map(record => record.CategoryPlaceNumber).ConvertUsing(CategoryPlaceConverter);
            Map(record => record.RaceTime).ConvertUsing(RaceTimeConverter);
            //Map(m => m.City).Name("miejscowosc");
            //Map(m => m.Subcategory).Name("rower");
            //Map(m => m.Distance).Name("nazwa_dystansu");
            //Map(m => m.Time).Name("czas_przejazdu"); // TODO: Write converter
            //Map(m => m.LapsCount).Name("liczba_pomiarow");
            //Map(m => m.MeasurementTime).Name("pomiary_csv");

        }

        private string CategoryConverter(IReaderRow row)
        {
            string fullCategoryString = row.GetField<string>("kategoria");
            return fullCategoryString.Split(' ').Last();
        }

        private DateTime DateBirthConverter(IReaderRow row)
        {
            string dateString = row.GetField<string>("data_urodzenia");
            if (string.IsNullOrWhiteSpace(dateString))
                return DateTime.Today;
            return CsvColumnHelpers.ConvertStringToDateTime(dateString);
        }

        private int DistancePlaceConverter(IReaderRow row)
        {
            string numberString = row.GetField<string>("msc_open_csv");
            return CsvColumnHelpers.ConvertStringToInteger(numberString);
        }

        private int CategoryPlaceConverter(IReaderRow row)
        {
            string numberString = row.GetField<string>("msc_kat_csv");
            return CsvColumnHelpers.ConvertStringToInteger(numberString);
        }

        private int LapsCountConverter(IReaderRow row)
        {
            string numberString = row.GetField<string>("liczba_pomiarow");
            return CsvColumnHelpers.ConvertStringToInteger(numberString);
        }

        private TimeSpan RaceTimeConverter(IReaderRow row)
        {
            string timeString = row.GetField<string>("czas_przejazdu");
            return CsvColumnHelpers.ConvertStringToTimeSpan(timeString);
        }
    }
}
