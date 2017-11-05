using System.Data.Entity;

namespace BaseCore.DataBase
{
    public class NTimeDBContext : DbContext
    {
        public NTimeDBContext(string nameOrConnectionString = "NTime") : base(nameOrConnectionString)
        {
            Database.SetInitializer(new NTimeDBInitializer());
        }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<CompetitionType> CompetitionTypes { get; set; }

        public DbSet<AgeCategory> AgeCategories { get; set; }

        public DbSet<AgeCategoryTemplateCollection> AgeCategoryTemplateCollections { get; set; }

        public DbSet<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }

        public DbSet<Distance> Distances { get; set; }

        public DbSet<ExtraPlayerInfo> ExtraPlayerInfos { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<ReaderOrder> ReaderOrders { get; set; }

        public DbSet<TimeRead> TimeReads { get; set; }

    }
}