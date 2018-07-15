using BaseCore.Csv.Converters;
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
            Map(m => m.LastName).Name("nazwisko");
            Map(m => m.FirstName).Name("imie");
            Map(m => m.City).Name("miejscowosc");
            Map(m => m.BirthDate).ConvertUsing(DateBirthConverter);
            Map(m => m.FullCategory).Name("kategoria");
            //Map(m => m.Subcategory).Name("rower");
            //Map(m => m.Distance).Name("nazwa_dystansu");
            //Map(m => m.Time).Name("czas_przejazdu"); // TODO: Write converter
            //Map(m => m.DistancePlaceNumber).Name("msc_open_csv");
            //Map(m => m.CategoryPlaceNumber).Name("msc_kat_csv");
            //Map(m => m.LapsCount).Name("liczba_pomiarow");
            //Map(m => m.MeasurementTime).Name("pomiary_csv");

        }

        private DateTime DateBirthConverter(IReaderRow row)
        {
            string dateString = row.GetField<string>("data_urodzenia");
            return CsvColumnConverters.ConvertStringToDateTime(dateString);
        }
    }
}
