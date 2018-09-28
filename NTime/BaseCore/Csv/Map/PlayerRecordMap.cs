using System;
using BaseCore.Csv.Helpers;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BaseCore.Csv
{
    public sealed class PlayerRecordMap : ClassMap<PlayerRecord>
    {
        public PlayerRecordMap()
        {
            Map(record => record.StartNumber).ConvertUsing(StartNumberConverter);
            Map(record => record.FirstName).Name("imie");
            Map(record => record.LastName).Name("nazwisko");
            Map(record => record.City).Name("miejscowosc");
            Map(record => record.Team).Name("klub");
            Map(record => record.BirthDate).ConvertUsing(BirthDateConverter);
            Map(record => record.IsMale).ConvertUsing(IsMaleConverter);
            Map(record => record.StringAditionalInfo).Name("rower");
            Map(record => record.StartTime).Name("czas_startu");
            Map(record => record.StringDistance).Name("nazwa_dystansu");
            //Map(m => m.ExtraData).Name("kolumny_dodatkowe");
        }

        private bool IsMaleConverter(IReaderRow row)
        {
            string maleString = row.GetField<string>("plec");
            return CsvColumnHelpers.ConvertStringToIsMale(maleString);
        }

        private DateTime BirthDateConverter(IReaderRow row)
        {
            string dateString = row.GetField<string>("data_urodzenia");
            return CsvColumnHelpers.ConvertStringToDateTime(dateString);
        }

        private int StartNumberConverter(IReaderRow row)
        {
            string integerString = row.GetField<string>("nr_startowy");
            return CsvColumnHelpers.ConvertStringToInteger(integerString);
        }
    }
}