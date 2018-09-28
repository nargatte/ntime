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
    public class AgeCategoryMap : ClassMap<AgeCategoryRecord>
    {
        private readonly string nameColumn = "nazwa";
        private readonly string yearFromColumn = "rocznik_od";
        private readonly string yearToColumn = "rocznik_do";
        private readonly string sexColumn = "plec";
        private readonly string distanceNameColumn = "nazwa_dystansu";
        public AgeCategoryMap()
        {
            Map(record => record.Name).Name(nameColumn);
            Map(record => record.YearFrom).Name(yearFromColumn).ConvertUsing(YearFromConverter);
            Map(record => record.YearTo).Name(yearToColumn).ConvertUsing(YearToConverter);
            Map(record => record.IsMale).Name(sexColumn).ConvertUsing(IsMaleConverter).ConvertUsing(record => record.IsMale ? "M" : "K");
            Map(record => record.DistanceName).Name(distanceNameColumn);
        }

        private int YearFromConverter(IReaderRow row)
        {
            string yearString = row.GetField<string>(yearFromColumn);
            return CsvColumnHelpers.ConvertStringToInteger(yearString);
        }

        private int YearToConverter(IReaderRow row)
        {
            string yearString = row.GetField<string>(yearToColumn);
            return CsvColumnHelpers.ConvertStringToInteger(yearString);
        }

        private bool IsMaleConverter(IReaderRow row)
        {
            string sexString = row.GetField<string>(sexColumn);
            return CsvColumnHelpers.ConvertStringToIsMale(sexString);
        }
    }
}
