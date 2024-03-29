﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BaseCore.DataBase;
using BaseCore.PlayerFilter;
using BaseCore.TimesProcess;
using NUnit.Framework;
using System.Text;

namespace BaseCoreTests
{
    [TestFixture]
    public class IntegrationTests
    {
        [Test]
        public void Sort()
        {
            var cp = new ContextProvider();
            var cr = new CompetitionRepository(cp);
            var pr = new PlayerRepository(cp, new Competition() { Id = 1 });


            bool argumentExceptionThrown = false;
            var thrownException = Assert.Throws<AggregateException>(() =>
            {
                var result = pr.GetAllByFilterAsync(
                new PlayerFilterOptions() { Query = "", ExtraDataSortIndex = 1, PlayerSort = PlayerSort.ByExtraData, DescendingSort = true }, 0,
                1000).Result;
            });
            thrownException.Handle((ex) =>
            {
                if(ex is ArgumentException)
                {
                    argumentExceptionThrown = true;
                    return true;
                }
                else
                {
                    argumentExceptionThrown = false;
                    return false;
                }
            });

            Assert.AreEqual(true, argumentExceptionThrown);
        }

        [Test]
        public async Task LoadCsvs()
        {

            var cp = new ContextProvider();
            var cr = new CompetitionRepository(cp);
            bool addExtraColumns = true;
            var sampleCompetition = new Competition($"Zawody Integracyjne {DateTime.Now:g}", DateTime.Now, "Integration City", null, null, null);
            if (addExtraColumns)
                sampleCompetition.ExtraDataHeaders = "Pierwsza|Druga|Trzecia";
            var com = await cr.AddAsync(sampleCompetition);
            var pr = new PlayerRepository(cp, com);
            var eifr = new SubcategoryRepository(cp, com);
            var dr = new DistanceRepository(cp, com);
            var akr = new AgeCategoryRepository(cp, com);
            var gr = new GateRepository(cp, com);
            var acdr = new AgeCategoryDistanceRepository(cp, com);

            await eifr.AddRangeAsync(new[]
            {
                new Subcategory("inny", "i"),
                new Subcategory("szosowy", "s")
            });

            await dr.AddRangeAsync(new[]
            {
                new Distance("MINI", 0, DistanceTypeEnum.DeterminedLaps) {LapsCount = 1},
                new Distance("+RODZINNY", 0, DistanceTypeEnum.DeterminedLaps) {LapsCount = 2},
                new Distance("MEGA", 0, DistanceTypeEnum.DeterminedLaps) {LapsCount = 2},
                new Distance("RODZINNY", 0, DistanceTypeEnum.DeterminedLaps) {LapsCount = 1},
                new Distance("GIGA", 0, DistanceTypeEnum.DeterminedDistance) {LapsCount = 3}
            });
            var ds = await dr.GetAllAsync();

            await akr.AddRangeAsync(new[]
            {
                new AgeCategory("Młodzik", 2001, DateTime.Today.Year, true),
                new AgeCategory("Junior", 1996, 2000, true),
                new AgeCategory("Starszak", 1986, 1995, true),
                new AgeCategory("Sernior", 1900, 1985, true),
                new AgeCategory("Młodziczka", 2001, DateTime.Today.Year, false),
                new AgeCategory("Juniorka", 1996, 2000, false),
                new AgeCategory("Starszaczka", 1986, 1995, false),
                new AgeCategory("Seniorka", 1900, 1985, false),
                new AgeCategory("Inna kategoria M", 0, int.MaxValue, true), 
                new AgeCategory("Inna kategoria K", 0, int.MaxValue, false),
            });
            var ags = await akr.GetAllAsync();

            bool b = true;
            foreach (Distance distance in ds)
            {
                foreach (AgeCategory ageCategory in ags)
                {
                    b = !b;
                    if (b) continue;
                    await acdr.AddAsync(new AgeCategoryDistance() {AgeCategoryId = ageCategory.Id, DistanceId = distance.Id});
                }
            }
            

            var g1 = await gr.AddAsync(new Gate("Pierwszy", 1));
            var lir1 = new TimeReadsLogInfoRepository(cp, g1);
            var g2 = await gr.AddAsync(new Gate("Drugi", 2));
            var lir2 = new TimeReadsLogInfoRepository(cp, g2);


            var darr = await dr.GetAllAsync();

            var ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "GIGA"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g2),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "MEGA"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g2),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "MINI"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g2),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "+RODZINNY"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            ror = new GateOrderItemRepository(cp, darr.FirstOrDefault(d => d.Name == "RODZINNY"));
            await ror.ReplaceByAsync(new[]
            {
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1),
                new Tuple<GatesOrderItem, Gate>(new GatesOrderItem(1800), g1)
            });

            TimeProcessManager timeProcess = new TimeProcessManager(com);

            string mainPath = Path.GetTempPath();

            Dictionary<string, string> filesDictionary = new Dictionary<string, string>();
            filesDictionary.Add("Zawodnicy.csv", Properties.Resources.Zawodnicy);
            filesDictionary.Add("log1.csv", Properties.Resources.log1);
            filesDictionary.Add("log2.csv", Properties.Resources.log2);
            filesDictionary.Add("log3.csv", Properties.Resources.log3);

            foreach (KeyValuePair<string, string> dpv in filesDictionary)
            {
                if (File.Exists(mainPath +  dpv.Key))
                    File.Delete(mainPath +  dpv.Key);

                using (FileStream fs = File.Create(mainPath + dpv.Key))
                {
                    byte[] tb = new UTF8Encoding(true).GetBytes(dpv.Value);
                    fs.Write(tb, 0, tb.Length);
                }
            }

            await pr.ImportPlayersAsync(mainPath +
                "Zawodnicy.csv");
            //await lir1.AddAsync(new TimeReadsLogInfo() { Path = mainPath + "log1.csv" });
            //await lir1.AddAsync(new TimeReadsLogInfo() { Path = mainPath + "log2.csv" });
            //await lir2.AddAsync(new TimeReadsLogInfo() { Path = mainPath + "log3.csv" });


            //var pfo = new PlayerFilterOptions { Query = "500" };
            //var p = (await pr.GetAllByFilterAsync(
            //    pfo, 0, 1)).Item1[0];

            //await timeProcess.ProcessSingleAsync(p);
            //await timeProcess.ProcessAllAsync();

            //await pr.ImportTimeReadsFromSourcesAsync();

            //await timeProcess.ProcessAllAsync();

            //await pr.UpdateFullCategoryAllAsync();

            //await pr.UpdateRankingAllAsync();

            //p.Distance = darr.FirstOrDefault(d => d.Name == "GIGA");
            //await pr.UpdateAsync(p, p.Distance, p.Subcategory);

            //await pr.UpdateFullCategoryAllAsync();

            //pfo = new PlayerFilterOptions
            //{
            //    PlayerSort = PlayerSort.ByRank,
            //    CompleatedCompetition = true,
            //    HasVoid = false,
            //    WithoutStartTime = false,
                
            //};
            //pfo.Query = "RODZ";
            //var pall = await pr.GetAllByFilterAsync(
            //    pfo, 0, 5000);

            //foreach (Player player in pall.Item1)
            //{
            //    Console.WriteLine(player);
            //    var trr = new TimeReadRepository(cp, player);
            //    var ts = await trr.GetAllAsync();
            //}
        }



    }
}