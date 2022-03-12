namespace PlatformService.Dtos
{
    public class PlatformReadDto
    {
        // Copy pasta from Platform.cs - the good news is we could change our Platform.cs (internal repo) if we like and as long as we
        // Note that since it's a read Dto we don't keep the data annotations [Key] etc.
        // Private Key for our Database
        public int Id { get; set; }
        
        // Name of Platform
        public string Name { get; set; }
        
        // Vendor 
        public string Publisher { get; set; }

        // Rate Card 
        public string Cost { get; set; }

    }
}