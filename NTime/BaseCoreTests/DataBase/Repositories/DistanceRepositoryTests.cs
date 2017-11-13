﻿using System;
using System.Threading.Tasks;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests.DataBase
{
    public class DistanceRepositoryCompetitionIdTests : RepositoryCompetitionIdTests<Distance>
    {
        protected override Distance[] InitialItems { get; set; } =
        {
            new Distance("Krótki", 10, new DateTime(2000, 1, 1, 16, 30, 0), DistanceTypeEnum.DeterminedLaps),
            new Distance("Średni", 20, new DateTime(2000, 1, 1, 13, 45, 0), DistanceTypeEnum.DeterminedLaps),
            new Distance("Długi", 30, new DateTime(2000, 1, 1, 8, 5, 0), DistanceTypeEnum.DeterminedLaps),
            new Distance("Maraton", 40, new DateTime(2000, 1, 1, 9, 50, 0), DistanceTypeEnum.DeterminedLaps)
        };

        protected override Repository<Distance> Repository { get; set; }

        protected override Task BeforeDataSetUp(NTimeDBContext ctx)
        {
            DistanceRepository dr = new DistanceRepository(ContextProvider, InitialCompetition);
            Repository = dr;
            return base.BeforeDataSetUp(ctx);
        }

        protected override bool TheSameData(Distance entity1, Distance entity2)
        {
            if (entity1.Name != entity2.Name) return false;
            if (entity1.Length != entity2.Length) return false;
            if (entity1.StartTime != entity2.StartTime) return false;
            if (entity1.CompetitionId != entity2.CompetitionId) return false;
            return true;
        }

        protected override bool SortTester(Distance before, Distance after) => String.CompareOrdinal(before.Name, after.Name) <= 0;

    }
}