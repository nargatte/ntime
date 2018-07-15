using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Converters
{
    internal static class CsvColumnConverters
    {
        public static DateTime ConvertStringToDateTime(string fieldString)
        {
            if (DateTime.TryParse(fieldString, out DateTime dateTime))
                return dateTime;
            if (Int32.TryParse(fieldString, out int year))
                return new DateTime(year, 1, 1);

            throw new Exception("Wrong date format");
        }

        public static int ConvertStringToInteger(string fieldString)
        {
            if (Int32.TryParse(fieldString, out int result))
                return result;

            throw new ArgumentException("This is not a number");
        }

        public static bool ConvertStringToIsMale(string fieldString)
        {
            if (fieldString == "M") return true;
            if (fieldString == "K") return false;
            throw new ArgumentException("Wrong SexString");
        }
    }
}
