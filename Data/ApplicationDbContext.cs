using photo_gallery_api;
using photo_gallery_api.Entities;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public readonly IWebHostEnvironment _hostingEnvironment;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IWebHostEnvironment hostingEnvironment) : base(options)
    {
        _hostingEnvironment = hostingEnvironment;
    }
    public DbSet<Image> Images { get; set; }    
    public DbSet<User> Users { get; set; }
    public DbSet<Favourite> Favourites { get; set; }

    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Favourite>().HasKey(f => new { f.UserId, f.ImageId });

        modelBuilder.Entity<Image>().HasData(SeedHelper());
    }

    //helper for seeding database
    public IEnumerable<Image> SeedHelper()
    {
        string[] imagePaths = Directory.GetFiles(Path.Combine(_hostingEnvironment.WebRootPath, "Images"));
        List<Image> images = new List<Image>();
        int id = 1;
        foreach (string imagePath in imagePaths)
        {
            images.Add(new Image { ImageId = id, ImageName = Path.GetFileName(imagePath), ImagePath="Images/" + Path.GetFileName(imagePath) }) ;
            id++;
        }
        return images;
    }
}