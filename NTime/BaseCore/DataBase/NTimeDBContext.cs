using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class NTimeDBContext : DbContext
    {
        public NTimeDBContext(string nameOrConnectionString = "name=Test") : base(nameOrConnectionString)
        {
            Database.SetInitializer(new NTimeDBInitializer());
        }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<CompetitionType> CompetitionTypes { get; set; }

        public DbSet<AgeCategory> AgeCategories { get; set; }

        public DbSet<AgeCategoryCollection> AgeCategoryCollections { get; set; }

        public DbSet<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }

        public DbSet<Distance> Distances { get; set; }

        public DbSet<ExtraPlayerInfo> ExtraPlayerInfo { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<ReaderOrder> ReaderOrders { get; set; }

        public DbSet<TimeRead> TimeReads { get; set; }

        public static void ContextDo(Action<NTimeDBContext> action, string nameOrConnectionString = null)
        {
            if (nameOrConnectionString == null)
            {
                using (var context = new NTimeDBContext())
                {
                    action(context);
                }
            }
            else
            {
                using (var context = new NTimeDBContext(nameOrConnectionString))
                {
                    action(context);
                }
            }
        }

        public static async Task ContextDoAsync(Func<NTimeDBContext, Task> action, string nameOrConnectionString = null)
        {
            if (nameOrConnectionString == null)
            {
                using (var context = new NTimeDBContext())
                {
                    await action(context);
                }
            }
            else
            {
                using (var context = new NTimeDBContext(nameOrConnectionString))
                {
                    await action(context);
                }
            }
        }
    }
}