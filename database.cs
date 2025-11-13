#:package LoremNET@2.0.0
#:package Microsoft.EntityFrameworkCore.Design@10.0.0
#:package Microsoft.EntityFrameworkCore.Sqlite@10.0.0
#:property PublishAot=false

using Microsoft.EntityFrameworkCore;


using var db = new DBContext();
db.Database.EnsureCreated();

//Create 200 new campgrounds with random sites and data using LoremNET
for (int i = 0; i < 200; i++)
{
    var campground = new Campground
    {
        Name = LoremNET.Lorem.Words(2),
        SeasonStart = DateOnly.FromDateTime(DateTime.Now.AddMonths(new Random().Next(0, 6))),
        SeasonEnd = DateOnly.FromDateTime(DateTime.Now.AddMonths(new Random().Next(6, 12))),
        Sites = new List<Site>()
    };

    int numberOfSites = new Random().Next(5, 20);
    for (int j = 0; j < numberOfSites; j++)
    {
        var site = new Site
        {
            SiteNumber = j + 1,
            MaxOccupancy = new Random().Next(1, 10),
            IsAccessible = new Random().Next(0, 2) == 1,
            MaxRVLength = new Random().Next(0, 40),
            DailyFee = Math.Round((decimal)(new Random().NextDouble() * 100), 2)
        };
        campground.Sites.Add(site);
    }

    db.Campgrounds.Add(campground);
}

await db.SaveChangesAsync();

//Get campgrounds with a RV length of at least 39
var suitableCampgrounds = await db.Campgrounds
    .Where(cg => cg.Sites.Any(s => s.MaxRVLength >= 39))
    .ToListAsync();

foreach (var campground in suitableCampgrounds)
{
    //List the campground name and sites that meet the criteria
    foreach (var site in campground.Sites.Where(s => s.MaxRVLength >= 39))
    {
        Console.WriteLine($"Campground Name: {campground.Name} | Site: {site.SiteNumber} | Max RV Length: {site.MaxRVLength} ft | Daily Fee: ${site.DailyFee}");
    }
}






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
    public DbSet<Campground> Campgrounds { get; set; } = null!;

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