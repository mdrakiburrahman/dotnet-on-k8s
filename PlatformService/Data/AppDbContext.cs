// For interacting with our Database
using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        
        // Database Set - asks DBContext to mirror our internal Class Models.Platforms to the Database
        // Note that we register this in Startup.cs
        public DbSet<Platform> Platforms { get; set; }
        public object Database { get; internal set; }
    }
}
