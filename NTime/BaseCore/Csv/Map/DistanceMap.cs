using BaseCore.Csv.Helpers;
using BaseCore.Csv.Records;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BaseCore.Csv.Map
{
    public class DistanceMap : ClassMap<DistanceRecord>
    {
        private readonly string nameColumn = "nazwa";
        private readonly string lengthColumn = "dlugosc";
        private readonly string gatesCountColumn = "punkty_pomiarowe";
        private readonly string readerColumn = "reader";
        private readonly string startTimeColumn = "czas_startu";
        private readonly string diffColumn = "nazwa";
        public DistanceMap()
        {
            Map(record => record.Name).Name(nameColumn);
            Map(record => record.Length).Name(lengthColumn).ConvertUsing(DistanceLengthConverter);
            Map(record => record.GatesCount).Name(gatesCountColumn).ConvertUsing(GatesCountConverter);
            Map(record => record.GatesOrder).Name(readerColumn).ConvertUsing(GatesOrderConverter).ConvertUsing(distance => string.Join("",distance.GatesOrder));
            Map(record => record.StartTime).Name(startTimeColumn);
            Map(record => record.Difference).Name(diffColumn).ConvertUsing(TimeDifferenceConverter);
        }

        private int DistanceLengthConverter(IReaderRow row)
        {
            string distanceLengthString = row.GetField<string>(lengthColumn);
            string numberString = Regex.Match(distanceLengthString, @"\d+").Value;
            return CsvColumnHelpers.ConvertStringToInteger(numberString, throwException: false);
        }

        private int GatesCountConverter(IReaderRow row)
        {
            string gatesCountString = row.GetField<string>(gatesCountColumn);
            return CsvColumnHelpers.ConvertStringToInteger(gatesCountString, throwException: true);
        }

        private IEnumerable<int> GatesOrderConverter(IReaderRow row)
        {
            string gatesOrderString = row.GetField<string>(readerColumn);
            return gatesOrderString.Select(c => CsvColumnHelpers.ConvertStringToInteger(c.ToString()));
        }

        private decimal TimeDifferenceConverter(IReaderRow row)
        {
            string timeDiffString = row.GetField<string>(diffColumn);
            return CsvColumnHelpers.ConvertStringToInteger(timeDiffString);
        }
    }
}
