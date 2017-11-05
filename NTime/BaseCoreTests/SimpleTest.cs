using System;
using System.Linq;
using BaseCore.DataBase;
using NUnit.Framework;

namespace BaseCoreTests
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void InitDatabase()
        {
            using (var context = new NTimeDBContext())
            {
                var c = new Competition
                {
                    Id = 78,
                    Name = "Comp",
                    City = "Poznań",
                    Description = "W poznaniu",
                    EventDate = new DateTime(2017, 12, 3),
                    CompetitionTypeId = (int)CompetitionTypeEnum.Fastest
                };
                context.Competitions.Add(c);
                context.SaveChanges();
            }
            using (var ctx = new NTimeDBContext())
            {
                var o = ctx.Competitions.FirstOrDefault();
                Assert.IsTrue(o.CompetitionType == CompetitionTypeEnum.Fastest);
            }
            Assert.Pass();
        }
    }
}