using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_srd.Models
{
    public class RuleEntry
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        public string? Prerequisite { get; set; }

        public string? Cost { get; set; }

        public double? Weight { get; set; }

        public string? DamageDice { get; set; }

        public string? DamageType { get; set; }

        public bool IsSRD { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int EditionId { get; set; }

        // Navigation
        public Edition Edition { get; set; } = null!;

        [NotMapped]
        public string SourceURL => $"/api/rules/{Id}";
    }
}
