using System.ComponentModel.DataAnnotations;

namespace dnd_srd.Models
{
    public class Action
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public int? AttackBonus { get; set; }

        public string? DamageDice { get; set; }

        public string? DamageType { get; set; }

        [Required]
        [StringLength(50)]
        public string ActionType { get; set; } = string.Empty;

        // Foreign Key
        public int MonsterId { get; set; }

        // Navigation
        public Monster Monster { get; set; } = null!;
    }
}
