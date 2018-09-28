using CsvHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv.Helpers
{
    internal static class CsvColumnHelpers
    {
        public static DateTime ConvertStringToDateTime(string fieldString)
        {
            if (DateTime.TryParse(fieldString, out DateTime dateTime))
                return dateTime;
            if (Int32.TryParse(fieldString, out int year))
                return new DateTime(year, 1, 1);

            throw new Exception("Wrong date format");
        }

        public static int ConvertStringToInteger(string fieldString, bool throwException = true)
        {
            if (string.IsNullOrWhiteSpace(fieldString))
                return -1;
            if (Int32.TryParse(fieldString, out int result))
                return result;
            if (throwException)
                throw new ArgumentException("This is not a number");
            else
                return -1;
        }

        public static bool ConvertStringToIsMale(string fieldString)
        {
            if (fieldString.ToUpper() == "M") return true;
            if (fieldString.ToUpper() == "K") return false;
            throw new ArgumentException("Wrong SexString");
        }

        public static TimeSpan ConvertStringToTimeSpan(string fieldString)
        {
            bool parsedCorrectly = TimeSpan.TryParse(fieldString, out TimeSpan time);
            if (parsedCorrectly)
                return time;
            else
                return TimeSpan.Zero;
        }
    }
}
