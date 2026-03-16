using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests
{
    public class RuleEntryRequestDto
    {
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

        [Required]
        public int EditionId { get; set; }
    }
}
