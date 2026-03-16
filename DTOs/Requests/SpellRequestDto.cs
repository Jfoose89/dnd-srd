using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests
{
    public class SpellRequestDto
    {
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

        [Required]
        public int EditionId { get; set; }

        [Required]
        public int ClassId { get; set; }
    }
}
