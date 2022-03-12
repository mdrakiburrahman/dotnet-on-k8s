namespace PlatformService.Dtos
{
    public class PlatformReadDto
    {
        // Copy pasta from Platform.cs
        
        // Private Key for our Database
        [Key]
        [Required]
        public int Id { get; set; }
        // Name of Platform
        [Required]
        public string Name { get; set; }
        // Vendor 
        [Required]
        public string Publisher { get; set; }

        // Rate Card 
        [Required]
        public string Cost { get; set; }

    }
}