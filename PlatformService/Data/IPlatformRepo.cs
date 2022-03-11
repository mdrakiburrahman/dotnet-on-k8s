// Interface for the Platform Repository
using System.Collections.Generic;
using PlatformService.Models;

namespace PlatformService.Data
{
    public interface IPlatformRepo
    {
        // Entity Framework needs us to save changes at the end of any add/deletes
        bool SaveChanges();

        // Get all Platforms as an IEnumerable
        IEnumerable<Platform> GetAllPlatforms();
    }
}