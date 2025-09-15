using Microsoft.EntityFrameworkCore;

namespace JsonInSQL
{
    public class Campground
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public DateOnly SeasonStart { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly SeasonEnd { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public List<Site> Sites { get; set; } = new List<Site>();
    }

    public class Site
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int SiteNumber { get; set; }
        public int MaxOccupancy { get; set; }
        public bool IsAccessible { get; set; }
        public int MaxRVLength { get; set; }
        public decimal DailyFee { get; set; }
    }

    public class DBContext : DbContext
    {
        public DbSet<Campground> Campgrounds { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=JsonInSQL-Demo.db");
            // optionsBuilder.UseSqlServer("Server=localhost;Database=SampleDb;Trusted_Connection=True;Encrypt=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<Campground>().ComplexProperty(b => b.Sites, b => b.ToJson());
            modelBuilder.Entity<Campground>().ComplexCollection(b => b.Sites, b => b.ToJson());
        }
    }
}
