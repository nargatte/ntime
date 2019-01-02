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
        private readonly string lastNameColumn = "nazwisko";
        private readonly string firstNameColumn = "imie";
        private readonly string birthDateColumn = "data_urodzenia";
        private readonly string ageCategoryColumn = "kategoria";
        private readonly string distancePlaceNumberColumn = "msc_open_csv";
        private readonly string categoryPlaceNumberColumn = "msc_kat_csv";
        private readonly string raceTimeColumn = "czas_przejazdu";
        public PlayerScoreMap()
        {
            Map(record => record.LastName).Name(lastNameColumn);
            Map(record => record.FirstName).Name(firstNameColumn);
            Map(record => record.BirthDate).Name(birthDateColumn).ConvertUsing(DateBirthConverter);
            Map(record => record.AgeCategory).Name(ageCategoryColumn).ConvertUsing(CategoryConverter);
            Map(record => record.DistancePlaceNumber).Name(distancePlaceNumberColumn).ConvertUsing(DistancePlaceConverter);
            Map(record => record.CategoryPlaceNumber).Name(categoryPlaceNumberColumn).ConvertUsing(CategoryPlaceConverter);
            Map(record => record.RaceTime).Name(raceTimeColumn).ConvertUsing(RaceTimeConverter);
            //Map(m => m.City).Name("miejscowosc");
            //Map(m => m.Subcategory).Name("rower");
            //Map(m => m.Distance).Name("nazwa_dystansu");
            //Map(m => m.Time).Name("czas_przejazdu"); // TODO: Write converter
            //Map(m => m.LapsCount).Name("liczba_pomiarow");
            //Map(m => m.MeasurementTime).Name("pomiary_csv");

        }

        private string CategoryConverter(IReaderRow row)
        {
            string fullCategoryString = row.GetField<string>(ageCategoryColumn).ToUpper();
            return fullCategoryString.Split(' ').Last();
        }

        private DateTime DateBirthConverter(IReaderRow row)
        {
            string dateString = row.GetField<string>(birthDateColumn);
            if (string.IsNullOrWhiteSpace(dateString))
                return DateTime.Today;
            return CsvColumnHelpers.ConvertStringToDateTime(dateString);
        }

        private int DistancePlaceConverter(IReaderRow row)
        {
            string numberString = row.GetField<string>(distancePlaceNumberColumn);
            return CsvColumnHelpers.ConvertStringToInteger(numberString);
        }

        private int CategoryPlaceConverter(IReaderRow row)
        {
            string numberString = row.GetField<string>(categoryPlaceNumberColumn);
            return CsvColumnHelpers.ConvertStringToInteger(numberString);
        }

        private int LapsCountConverter(IReaderRow row)
        {
            string numberString = row.GetField<string>("liczba_pomiarow");
            return CsvColumnHelpers.ConvertStringToInteger(numberString);
        }

        private TimeSpan RaceTimeConverter(IReaderRow row)
        {
            string timeString = row.GetField<string>(raceTimeColumn);
            return CsvColumnHelpers.ConvertStringToTimeSpan(timeString);
        }
    }
}
