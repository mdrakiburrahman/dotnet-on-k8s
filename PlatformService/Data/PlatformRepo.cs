using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
    // Class for the Platform Repository
    // Implements the interface IPlatformRepo
    public class PlatformRepo : IPlatformRepo
    {
        private readonly AppDbContext _context;
        // Dependency Injection
        public PlatformRepo(AppDbContext context)
        {
            _context = context; // Private field for dependency injection - declared above
        }
        // Create Platform
        public void CreatePlatform(Platform platform)
        {
            throw new System.NotImplementedException();
        }
        // Entity Framework needs us to save changes at the end of any add/deletes
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        // Get all Platforms as an IEnumerable
        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new System.NotImplementedException();
        }

        // Get individual Platform
        public Platform GetPlatformById(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
