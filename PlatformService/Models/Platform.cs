using System.ComponentModel.DataAnnotations;

// Internal view of our data
namespace PlatformService.Models
{
    public class Platform
    {
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