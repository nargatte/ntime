using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using BaseCore.Csv;
using CsvHelper.Configuration;
using NUnit.Framework;

namespace BaseCoreTests.Csv
{
    [TestFixture]
    public abstract class CsvImporterTests<T, TM>
        where T : class
        where TM : ClassMap<T>
    {

        [Test]
        public void GetAllRecordsFromStreamTest()
        {
            T[] items;
            using (TextReader reader = new StringReader(CsvString))
            {
                CsvImporter<T, TM> importer = new CsvImporter<T, TM>("");
                items = importer.GetAllRecordsFromStream(reader);
            }
            Assert.Equals(items.Length, NumberOfExpectedItems);
        }

        protected abstract int NumberOfExpectedItems { get; }

        protected abstract string CsvString { get; }
    }
}