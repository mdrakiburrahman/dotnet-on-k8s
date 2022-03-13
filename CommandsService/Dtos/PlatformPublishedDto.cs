namespace CommandsService.Dtos
{
    public class PlatformPublishedDto
    {
        // Identical to Platform - mapping is done in CommandsProfile.cs

        // We will have to map this to ExternalID field
        public int Id { get; set; }
        public string Name { get; set; }
        public string Event { get; set; }
    }
}