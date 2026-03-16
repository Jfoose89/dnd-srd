using System.ComponentModel.DataAnnotations;

namespace dnd_srd.Models
{
    public class MonsterType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        // Navigation
        public ICollection<Monster> Monsters { get; set; } = new List<Monster>();
    }
}
