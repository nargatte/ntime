using System;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    [TestFixture]
    public class DistanceRepositoryCompetitionIdTests : RepositoryCompetitionIdTests<Distance>
    {
        protected override Distance[] InitialItems { get; set; } =
        {
            new Distance("Krótki", 10, new DateTime(2000, 1, 1, 16, 30, 0)),
            new Distance("Średni", 20, new DateTime(2000, 1, 1, 13, 45, 0)),
            new Distance("Długi", 30, new DateTime(2000, 1, 1, 8, 5, 0)),
            new Distance("Maraton", 40, new DateTime(2000, 1, 1, 9, 50, 0))
        };

        protected override Repository<Distance> Repository => new DistanceRepository(InitialCompetition);

        protected override bool TheSameData(Distance entity1, Distance entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.Length != entity2.Length) return false;
            if (entity1.StartTime != entity2.StartTime) return false;
            if (entity1.CompetitionId != entity2.CompetitionId) return false;
            return true;
        }

        protected override bool SortTester(Distance before, Distance after) => String.CompareOrdinal(before.Name, after.Name) > 0;
    }
}