using System.ComponentModel.DataAnnotations;

namespace dnd_srd.Models
{
    public class Environment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Foreign Key
        public int MonsterId { get; set; }

        // Navigation
        public Monster Monster { get; set; } = null!;
    }
}
