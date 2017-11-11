using BaseCore.Csv;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace BaseCoreTests.Csv
{
    [TestFixture]
    public class CsvImporterPlayersTests : CsvImporterTests<PlayerRecord, PlayerRecordMap>
    {
        protected override int NumberOfExpectedItems => 602;
        protected override string CsvString => Properties.Resources.Players;
    }
}