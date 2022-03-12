using System.ComponentModel.DataAnnotations;

namespace PlatformService.Dtos
{
    public class PlatformCreateDto
    {   
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