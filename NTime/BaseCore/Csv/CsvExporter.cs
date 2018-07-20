using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Csv
{
    public class CsvExporter<T, TM>
        where T : class
        where TM : ClassMap<T>
    {
        private readonly string _fileName;
        private readonly char _delimiter;

        public CsvExporter(string fileName)
        {
            _fileName = fileName;
        }

        public CsvExporter(string fileName, char delimiter = ';') : this(fileName)
        {
            _delimiter = delimiter;
        }

        public Task<bool> SaveAllRecordsAsync(IEnumerable<T> records) =>
        Task.Factory.StartNew(() =>
        {
            bool writeSuccessfull = false;
            using (TextWriter textWriter = File.CreateText(_fileName))
            {
                writeSuccessfull = SaveAllRecordsFromStream(textWriter, records);
            }
            return writeSuccessfull;
        });

        public bool SaveAllRecordsFromStream(TextWriter textWriter, IEnumerable<T> records)
        {
            var csv = new CsvWriter(textWriter);
            csv.Configuration.RegisterClassMap<TM>();
            csv.Configuration.Delimiter = _delimiter.ToString();
            try
            {
                csv.WriteRecords(records);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Could not write CSV: {e.Message}");
                return false;
            }
            return true;
        }
    }
}
