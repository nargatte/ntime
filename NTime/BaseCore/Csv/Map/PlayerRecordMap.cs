using System;
using BaseCore.Csv.Converters;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BaseCore.Csv
{
    public sealed class PlayerRecordMap : ClassMap<PlayerRecord>
    {
        public PlayerRecordMap()
        {
            Map(m => m.StartNumber).ConvertUsing(StartNumberConverter);
            Map(m => m.FirstName).Name("imie");
            Map(m => m.LastName).Name("nazwisko");
            Map(m => m.City).Name("miejscowosc");
            Map(m => m.Team).Name("klub");
            Map(m => m.BirthDate).ConvertUsing(BirthDateConverter);
            Map(m => m.IsMale).ConvertUsing(IsMaleConverter);
            Map(m => m.StringAditionalInfo).Name("rower");
            Map(m => m.StartTime).Name("czas_startu");
            Map(m => m.StringDistance).Name("nazwa_dystansu");
            Map(m => m.ExtraData).Name("kolumny_dodatkowe");
        }

        private bool IsMaleConverter(IReaderRow row)
        {
            string maleString = row.GetField<string>("plec");
            return CsvColumnConverters.ConvertStringToIsMale(maleString);
        }

        private DateTime BirthDateConverter(IReaderRow row)
        {
            string dateString = row.GetField<string>("data_urodzenia");
            return CsvColumnConverters.ConvertStringToDateTime(dateString);
        }

        private int StartNumberConverter(IReaderRow row)
        {
            string integerString = row.GetField<string>("nr_startowy");
            return CsvColumnConverters.ConvertStringToInteger(integerString);
        }
    }
}