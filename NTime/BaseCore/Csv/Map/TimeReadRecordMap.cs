using System;
using CsvHelper;
using CsvHelper.Configuration;

namespace BaseCore.Csv
{
    public sealed class TimeReadRecordMap : ClassMap<TimeReadRecord>
    {
        public TimeReadRecordMap()
        {
            Map(record => record.StartNumber).ConvertUsing(StartNumberConverter);
            Map(record => record.Time).ConvertUsing(ConvertTime);
        }

        private decimal ConvertTime(IReaderRow row)
        {
            string[] numberStrings = row.GetField<string>("time").Split('.', ':');
            if (numberStrings.Length != 4)
                throw new ArgumentException("Wrong time string");

            int[] numbers = new int[numberStrings.Length];

            for (int i = 0; i < numberStrings.Length; i++)
            {
                if (!Int32.TryParse(numberStrings[i], out numbers[i]))
                    throw new ArgumentException("Int prase fail");
            }

            int seconds = numbers[0] * 60 * 60;
            seconds += numbers[1] * 60;
            seconds += numbers[2];

            string decimalString = seconds.ToString() + ',' + numbers[3];

            if (!Decimal.TryParse(decimalString, out decimal ret))
                throw new ArgumentException("Decimal prase fail");

            return ret;
        }

        private int StartNumberConverter(IReaderRow row)
        {
            string s = row.GetField<string>("num");

            if (Int32.TryParse(s, out int result))
                return result;

            return -1;
        }
    }
}