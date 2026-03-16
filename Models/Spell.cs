using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_srd.Models
{
    public class Spell
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 9)]
        public int Level { get; set; }

        [Required]
        [StringLength(100)]
        public string School { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CastingTime { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Range { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Duration { get; set; } = string.Empty;

        public bool Concentration { get; set; }

        public bool Ritual { get; set; }

        [Required]
        [StringLength(50)]
        public string Components { get; set; } = string.Empty;

        public bool IsSRD { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int EditionId { get; set; }
        public int ClassId { get; set; }

        // Navigation
        public Edition Edition { get; set; } = null!;
        public Class Class { get; set; } = null!;

        [NotMapped]
        public string SourceURL => $"/api/spells/{Id}";
    }
}
