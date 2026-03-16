using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests
{
    public class RaceRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0, 999)]
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

        [Required]
        public int EditionId { get; set; }
    }
}
