using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace BaseCore.DataBase
{
    public class NTimeDBContext : DbContext
    {
        public NTimeDBContext()
        {
            Database.SetInitializer(new NTimeDBInitializer());
        }

        public NTimeDBContext(string nameOrConnectionString = "NTime") : base(nameOrConnectionString)
        {
            Database.SetInitializer(new NTimeDBInitializer());
        }

        public DbSet<Competition> Competitions { get; set; }

        public DbSet<DistanceType> DistanceTypes { get; set; }

        public DbSet<AgeCategory> AgeCategories { get; set; }

        public DbSet<AgeCategoryCollection> AgeCategoryCollections { get; set; }

        public DbSet<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }

        public DbSet<Distance> Distances { get; set; }

        public DbSet<ExtraPlayerInfo> ExtraPlayerInfo { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<GatesOrderItem> GatesOrder { get; set; }

        public DbSet<TimeRead> TimeReads { get; set; }

        public DbSet<TimeReadType> TimeReadTypes { get; set; }

        public DbSet<Gate> Gates { get; set; }

        public DbSet<TimeReadsLogInfo> LogsSources { get; set; }

        public DbSet<OrganizerAccount> OrganizerAccounts { get; set; }

        public DbSet<PlayerAccount> PlayerAccounts { get; set; }
    }
}