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

    public CsvImporter(string fileName) => _fileName = fileName;

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
            CsvReader csv = new CsvReader(tr);
            csv.Configuration.RegisterClassMap<TM>();
            csv.Configuration.Delimiter = ";";
            return csv.GetRecords<T>().ToArray();
        }
    }
}