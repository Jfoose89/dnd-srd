using System.ComponentModel.DataAnnotations;

namespace dnd_srd.Models
{
    public class Trait
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;

        // Foreign Key
        public int MonsterId { get; set; }
        // Navigation
        public Monster Monster { get; set; } = null!;
    }
}
