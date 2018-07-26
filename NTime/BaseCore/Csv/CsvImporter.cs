using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;

namespace BaseCore.Csv
{
    public class CsvImporter<T, TM>
        where T : class
        where TM : ClassMap<T>
    {
        private readonly string _fileName;
        private readonly char _delimiter;

        public CsvImporter(string fileName, char delimiter = ',')
        {
            _fileName = fileName;
            _delimiter = delimiter;
        }

        public Task<T[]> GetAllRecordsAsync() =>
        Task.Factory.StartNew(() =>
        {
            T[] readRecords;
            using (TextReader reader = File.OpenText(_fileName))
            {
                readRecords = GetAllRecordsFromStream(reader);
            }
            return readRecords;
        });

        public T[] GetAllRecordsFromStream(TextReader tr)
        {
            var csv = new CsvReader(tr);
            csv.Configuration.RegisterClassMap<TM>();
            csv.Configuration.Delimiter = _delimiter.ToString();
            return csv.GetRecords<T>().ToArray();
        }
    }
}