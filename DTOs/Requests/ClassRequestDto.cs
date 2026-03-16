using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests
{
    public class ClassRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string HitDie { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string PrimaryAbility { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string SavingThrows { get; set; } = string.Empty;

        [Required]
        public string ArmorProficiencies { get; set; } = string.Empty;

        [Required]
        public string WeaponProficiencies { get; set; } = string.Empty;

        [Required]
        public int EditionId { get; set; }

        public int? ParentClassId { get; set; }
    }
}
