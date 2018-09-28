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
    public class PlacePointsMap : ClassMap<PlacePointsRecord>
    {
        public PlacePointsMap()
        {
            Map(record => record.Place).ConvertUsing(PlaceConverter);
            Map(record => record.Points).ConvertUsing(PointsConverter);
        }

        private int PlaceConverter(IReaderRow row)
        {
            string integerString = row.GetField<string>("place");
            return CsvColumnHelpers.ConvertStringToInteger(integerString);
        }

        private int PointsConverter(IReaderRow row)
        {
            string integerString = row.GetField<string>("points");
            return CsvColumnHelpers.ConvertStringToInteger(integerString);
        }
    }
}
