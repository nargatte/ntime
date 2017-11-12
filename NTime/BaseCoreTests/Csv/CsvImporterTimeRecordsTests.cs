using BaseCore.Csv;

namespace BaseCoreTests.Csv
{
    public class CsvImporterTimeRecordsTests : CsvImporterTests<TimeReadRecord, TimeReadRecordMap>
    {
        protected override int NumberOfExpectedItems => 3638;
        protected override string CsvString => Properties.Resources.Times;
    }
}