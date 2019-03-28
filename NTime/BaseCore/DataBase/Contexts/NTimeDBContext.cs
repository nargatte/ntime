using System.Data.Entity;

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

        public DbSet<AgeCategoryTemplate> AgeCategoryTemplates { get; set; }

        public DbSet<AgeCategoryTemplateItem> AgeCategoryTemplateItems { get; set; }

        public DbSet<Distance> Distances { get; set; }

        public DbSet<Subcategory> Subcategory { get; set; }

        public DbSet<Player> Players { get; set; }

        public DbSet<GatesOrderItem> GatesOrderItems { get; set; }

        public DbSet<TimeRead> TimeReads { get; set; }

        public DbSet<TimeReadType> TimeReadTypes { get; set; }

        public DbSet<Gate> Gates { get; set; }

        public DbSet<TimeReadsLogInfo> LogsSources { get; set; }

        public DbSet<OrganizerAccount> OrganizerAccounts { get; set; }

        public DbSet<PlayerAccount> PlayerAccounts { get; set; }

        public DbSet<AgeCategoryDistance> AgeCategoryDistances { get; set; }

        public DbSet<ExtraColumn> ExtraColumns { get; set; }

        public DbSet<ExtraColumnValue> ExtraColumnValues { get; set; }
    }
}