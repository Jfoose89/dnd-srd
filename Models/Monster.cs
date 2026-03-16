using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_srd.Models
{
    public class Monster
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public int HitPoints { get; set; }

        [Required]
        public int ArmorClass { get; set; }

        [Required]
        public int WalkSpeed { get; set; }
        public int? FlySpeed { get; set; }
        public int? SwimSpeed { get; set; }
        public int? BurrowSpeed { get; set; }

        [Required]
        public double ChallengeRating { get; set; }

        [Required]
        [StringLength(20)]
        public string Size { get; set; } = string.Empty;

        public string? Alignment { get; set; }

        public bool IsSRD { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Keys
        public int EditionId { get; set; }
        public int MonsterTypeId { get; set; }

        // Navigation properties
        public Edition Edition { get; set; } = null!;
        public MonsterType MonsterType { get; set; } = null!;
        public AbilityScores? AbilityScores { get; set; }
        public ICollection<Action> Actions { get; set; } = new List<Action>();
        public ICollection<Trait> Traits { get; set; } = new List<Trait>();
        public ICollection<Models.Environment> Environments { get; set; } = new List<Models.Environment>();

        // Derived - not stored in database
        [NotMapped]
        public string SourceURL => $"/api/monsters/{Id}";
    }
}
