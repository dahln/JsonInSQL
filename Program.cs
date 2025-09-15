using JsonInSQL;
using Microsoft.EntityFrameworkCore;


class Program
{
    static async Task Main()
    {
        using var db = new DBContext();
        db.Database.EnsureCreated();

        //Create 100 new campgrounds with random sites and data using LoremNET
        for (int i = 0; i < 20; i++)
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
    }
}
