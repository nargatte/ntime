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
            return new DateTime(2000, 1, 1, h, m, s);
        }

        public static decimal ToDecimal(this DateTime dateTime)
        {
            int seconds = dateTime.Hour * 60 * 60;
            seconds += dateTime.Minute * 60;
            seconds += dateTime.Second;

            string decimalString = seconds.ToString();

            decimal ret;

            if (!Decimal.TryParse(decimalString, out ret))
                throw new ArgumentException("Decimal prase fail");

            return ret;
        }
    }
}