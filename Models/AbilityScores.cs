using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dnd_srd.Models
{
    public class AbilityScores
    {
        public int Id { get; set; }
        public int MonsterId { get; set; }

        [Range(1, 30)]
        public int Strength { get; set; }

        [Range(1, 30)]
        public int Dexterity { get; set; }

        [Range(1, 30)]
        public int Constitution { get; set; }

        [Range(1, 30)]
        public int Intelligence { get; set; }

        [Range(1, 30)]
        public int Wisdom { get; set; }

        [Range(1, 30)]
        public int Charisma { get; set; }

        // Derived - computed in C#, not in stored database
        [NotMapped]
        public int StrengthModifier => (Strength - 10) / 2;

        [NotMapped]
        public int DexterityModifier => (Dexterity - 10) / 2;

        [NotMapped]
        public int ConstitutionModifier => (Constitution - 10) / 2;

        [NotMapped]
        public int IntelligenceModifier => (Intelligence - 10) / 2;

        [NotMapped]
        public int WisdomModifier => (Wisdom - 10) / 2;

        [NotMapped]
        public int CharismaModifier => (Charisma - 10) / 2;

        // Navigation
        public Monster Monster { get; set; } = null!;
    }
}
