using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {

        }
        // Pull from our Models
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<Command> Commands { get; set; }

        // Override EF relationships with explicit
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Platform>()
                .HasMany( p => p.Commands)
                .WithOne( p => p.Platform)
                .HasForeignKey ( p => p.PlatformId);
            
            modelBuilder
                .Entity<Command>()
                .HasOne( p => p.Platform)
                .WithMany( p => p.Commands )
                .HasForeignKey ( p => p.PlatformId);
        }
    }
}