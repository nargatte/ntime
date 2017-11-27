using System;
using BaseCore.DataBase;

namespace BaseCore.TimesProcess
{
    public static class TimeHelper
    {
        public static DateTime ToDateTime(this decimal @decimal)
        {
            int h = (int)(@decimal / (60 * 60));
            int m = (int)((@decimal - h * 60 * 60) / 60);
            int s = (int)(@decimal - h * 60 * 60 - m * 60);
            int ms = (int) ((@decimal - (int) @decimal) * 1000);
            DateTime dt = new DateTime(2000, 1, 1, h, m, s);
            return dt.AddMilliseconds(ms);
        }

        public static decimal ToDecimal(this DateTime dateTime)
        {
            int seconds = dateTime.Hour * 60 * 60;
            seconds += dateTime.Minute * 60;
            seconds += dateTime.Second;

            string decimalString = seconds.ToString();


            if (!Decimal.TryParse(decimalString, out decimal ret))
                throw new ArgumentException("Decimal prase fail");

            return ret;
        }

        public static bool TryConvertToDateTime(this string timeString, out DateTime dateTime)
        {
            dateTime = DateTime.Today;
            if (timeString is null)
                return false;
            string[] dividedString = timeString.Split(':');
            if (TryParseInt(dividedString[0], out int hours, 0, 23))
                return false;
            if (TryParseInt(dividedString[1], out int minutes, 0, 59))
                return false;
            var secondsString = dividedString[2].Split('.');
            int seconds = 0;
            int miliseconds = 0;
            if (secondsString.Length == 0 || secondsString.Length > 2)
                return false;
            if (secondsString.Length == 2)
            {
                if (TryParseInt(secondsString[1], out miliseconds, 0, 999))
                    return false;
            }
            if (TryParseInt(secondsString[0], out seconds, 0, 59))
                return false;

            dateTime = dateTime.AddHours(hours).AddMinutes(minutes).AddSeconds(seconds).AddMilliseconds(miliseconds);
            return true;
        }

        private static bool TryParseInt(string s, out int result, int minValue, int maxValue)
        {
            return (!int.TryParse(s, out result)) || result < minValue || result > maxValue;
        }

        public static string ConvertToString(this DateTime time)
        {
            return $"{time.TimeOfDay.Hours.ToString("00")}:{time.TimeOfDay.Minutes.ToString("00")}:{time.TimeOfDay.Seconds.ToString("00")}.{time.TimeOfDay.Milliseconds.ToString("000")}";
        }


    }
}