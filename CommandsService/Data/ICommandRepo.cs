using System.Collections.Generic;
using CommandsService.Models;

namespace CommandsService.Data
{
    public interface ICommandRepo
    {
        // Since we're using a InMem database
        bool SaveChanges();

        // Platforms: this is how we will add Platforms into our service
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);
        bool ExternalPlatformExists(int externalPlatformId);

        // Commands: These will be the commands we can execute on our Platform
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);
    }
}