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
        public int StrengthModifier => (int)Math.Floor((Strength - 10) / 2.0);

        [NotMapped]
        public int DexterityModifier => (int)Math.Floor((Dexterity - 10) / 2.0);

        [NotMapped]
        public int ConstitutionModifier => (int)Math.Floor((Constitution - 10) / 2.0);

        [NotMapped]
        public int IntelligenceModifier => (int)Math.Floor((Intelligence - 10) / 2.0);

        [NotMapped]
        public int WisdomModifier => (int)Math.Floor((Wisdom - 10) / 2.0);

        [NotMapped]
        public int CharismaModifier => (int)Math.Floor((Charisma - 10) / 2.0);

        // Navigation
        public Monster Monster { get; set; } = null!;
    }
}
