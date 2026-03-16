using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_srd.Models
{
    public class Class
    {
        public int Id { get; set; }

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

        public bool IsSRD { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int EditionId { get; set; }
        public int? ParentClassId { get; set; }

        // Navigation
        public Edition Edition { get; set; } = null!;
        public Class? ParentClass { get; set; }
        public ICollection<Class> Subclasses { get; set; } = new List<Class>();
        public ICollection<Spell> Spells { get; set; } = new List<Spell>();

        [NotMapped]
        public string SourceURL => $"/api/classes/{Id}";
    }
}
