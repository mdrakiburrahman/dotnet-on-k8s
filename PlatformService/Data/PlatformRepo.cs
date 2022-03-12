using System;
using System.Collections.Generic;
using System.Linq;
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
        public void CreatePlatform(Platform plat)
        {
            // Check that a valid object is passed in
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }
    
            _context.Platforms.Add(plat);
        }
        // Entity Framework needs us to save changes at the end of any add/deletes
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0); // >=0 means true
        }

        // Get all Platforms as an IEnumerable
        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _context.Platforms.ToList();
        }

        // Get individual Platform
        public Platform GetPlatformById(int id)
        {
            // Lambda expression says, find a p such that p.Id == id
            return _context.Platforms.FirstOrDefault(p => p.Id == id); // Returns the first Platform that matches the condition;  else null
        }
    }
}
