using System.ComponentModel.DataAnnotations;

namespace dnd_srd.Models
{
    public class Edition
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LicenseType { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<Monster> Monsters { get; set; } = new List<Monster>();
        public ICollection<Spell> Spells { get; set; } = new List<Spell>();
        public ICollection<Race> Races { get; set; } = new List<Race>();
        public ICollection<Class> Classes { get; set; } = new List<Class>();
        public ICollection<RuleEntry> RuleEntries { get; set; } = new List<RuleEntry>();
    }
}
