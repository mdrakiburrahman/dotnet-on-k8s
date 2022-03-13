namespace CommandsService.Dtos
{
    public class CommandReadDto
    {
        // Internal ID
        public int Id { get; set;}
        // How to do a particular activity; e.g. build a dotnet project
        public string HowTo { get; set; }
        // The CLI command to execute
        public string CommandLine { get; set; }
        // Foreign Key reference back to Platform
        public int PlatformId { get; set; }
    }
}