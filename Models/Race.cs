using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_srd.Models
{
    public class Race
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int Speed { get; set; }

        [Required]
        [StringLength(20)]
        public string Size { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string AbilityScoreBonus { get; set; } = string.Empty;

        [Required]
        public string Traits { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Languages { get; set; } = string.Empty;

        public bool IsSRD { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int EditionId { get; set; }

        // Navigation
        public Edition Edition { get; set; } = null!;

        [NotMapped]
        public string SourceURL => $"/api/races/{Id}";
    }
}
