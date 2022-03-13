using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Command
    {
        // Internal ID
        [Key]
        [Required]
        public int Id { get; set;}
        // How to do a particular activity; e.g. build a dotnet project
        [Required]
        public string HowTo { get; set; }
        // The CLI command to execute
        [Required]
        public string CommandLine { get; set; }
        // Foreign Key reference back to Platform
        [Required]
        public int PlatformId { get; set; }
        // Navigation property to Platform
        public Platform Platform { get; set; }
    }
}