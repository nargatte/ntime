using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using CsvHelper;

namespace BaseCore.Csv
{
    public class TimeReadImporter
    {
        private readonly string _fileName;
        private readonly int _readerNumber;

        public TimeReadImporter(string fileName, int readerNumber)
        {
            _fileName = fileName;
            _readerNumber = readerNumber;
        }

        public async Task<TimeRead[]> GetAllAsync()
        {
            TimeReadRecord[] readRecords = await GetAllRecordsAsync();
            return readRecords.Select(i => new TimeRead(ConvertTime(i.time), _readerNumber){StartNumber = i.num}).ToArray();
        }

        private Task<TimeReadRecord[]> GetAllRecordsAsync() =>
            Task.Factory.StartNew(() =>
            {
                TimeReadRecord[] readRecords = null; 
                using (TextReader reader = File.OpenText(_fileName))
                {
                    CsvReader csv = new CsvReader(reader);
                    readRecords = csv.GetRecords<TimeReadRecord>().ToArray();
                }
                return readRecords;
            });

        private decimal ConvertTime(string time)
        {
            string[] numberStrings = time.Split('.', ':');
            if(numberStrings.Length != 4)
                throw new ArgumentException("Wrong time string");

            int[] numbers = new int[numberStrings.Length];

            for (int i = 0; i < numberStrings.Length; i++)
            {
                if(!Int32.TryParse(numberStrings[i], out numbers[i]))
                    throw new ArgumentException("Int prase fail");
            }

            int seconds = numbers[0] * 60 * 60;
            seconds += numbers[1] * 60;
            seconds += numbers[2];

            string decimalString = seconds.ToString() + '.' + numbers[3];

            decimal ret;

            if(!Decimal.TryParse(decimalString, out ret))
                throw new ArgumentException("Decimal prase fail");

            return ret;
        }
    }
}