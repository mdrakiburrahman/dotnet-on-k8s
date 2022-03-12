using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data
{
    // This class is just for testing with some mock data quickly, not for PROD!
    public static class PrepDb
    {
        // This static class has 2 methods:
        // 1. Public: Set up a Database Context for us to use (not a Constructor)
        // 2. Private

        // Questions:
        // 1. Why aren't we using dependency injection?
        // A: Can't - static class

        // 2. Why DB Context and not PlatformRepo Class?
        // A: We want to reuse this to migrate to SQL Server, much easier directly over DBContext

        // We will call this from Startup.cs
        public static void PrepPopulation(IApplicationBuilder app)
        {
            // Create a Service Scope to create a DB Context
            using( var serviceScope = app.ApplicationServices.CreateScope())
            {
                // Call our Private Method to seed data
                SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
            }
        }

        private static void SeedData(AppDbContext context)
        {
            // Since it's staic and we can't use dependency injection, we receive the context directly
            if(!context.Platforms.Any())
            {
                // If there is no data, push some in
                Console.WriteLine("--> Seeding data...");

                context.Platforms.AddRange( // Adds a bunch of objects
                    new Platform() {Name="Dot Net", Publisher="Microsoft", Cost="Free"},
                    new Platform() {Name="SQL Server Express", Publisher="Microsoft",  Cost="Free"},
                    new Platform() {Name="Kubernetes", Publisher="Cloud Native Computing Foundation",  Cost="Free"}
                );

                // Save Changes
                context.SaveChanges();
            }  
            else
            {
                Console.WriteLine("--> We already have data"); // We'll never hit this since DB is in memory and data lost when we reboot, might see this in SQL Server though so we leave it
            }

        }

    }
}