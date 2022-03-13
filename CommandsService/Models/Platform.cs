using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandsService.Models
{
    public class Platform
    {   
        // Internal ID
        [Key]
        [Required]
        public int Id {get; set; }
        // For tracking Primary Key grabbed from Platform Service
        [Required]
        public int ExternalID  {get; set; }
        // Service Name
        [Required]
        public string Name { get; set; }
        // Collection of Commands objects defined in Commands.cs
        public ICollection<Command> Commands { get; set;} = new List<Command>();
    }
}