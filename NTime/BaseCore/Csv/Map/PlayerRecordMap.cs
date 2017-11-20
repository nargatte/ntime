using System;
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
            Map(m => m.BirthDate).ConvertUsing(DateBirthConverter);
            Map(m => m.IsMale).ConvertUsing(IsMaleConverter);
            Map(m => m.StringAditionalInfo).Name("rower");
            Map(m => m.StartTime).Name("czas_startu");
            Map(m => m.StringDistance).Name("nazwa_dystansu");
        }

        private bool IsMaleConverter(IReaderRow row)
        {
            string s = row.GetField<string>("plec");
            if (s == "M") return true;
            if (s == "K") return false;
            throw new ArgumentException("Wrong SexString");
        }

        private DateTime DateBirthConverter(IReaderRow row)
        {
            string s = row.GetField<string>("data_urodzenia");

            DateTime dt;
            if (DateTime.TryParse(s, out dt))
                return dt;
            int year;
            if(Int32.TryParse(s, out year))
                return new DateTime(year, 1, 1);

            throw new Exception("Wrong BirthDate format");
        }

        private int StartNumberConverter(IReaderRow row)
        {
            string s = row.GetField<string>("nr_startowy");

            int result;
            if (Int32.TryParse(s, out result))
                return result;

            return -1;
        }
    }
}