namespace dnd_srd.DTOs.Responses;

public class AbilityScoresDto
{
    public int Strength { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Charisma { get; set; }
    public int StrengthModifier { get; set; }
    public int DexterityModifier { get; set; }
    public int ConstitutionModifier { get; set; }
    public int IntelligenceModifier { get; set; }
    public int WisdomModifier { get; set; }
    public int CharismaModifier { get; set; }
}

public class ActionDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? AttackBonus { get; set; }
    public string? DamageDice { get; set; }
    public string? DamageType { get; set; }
    public string ActionType { get; set; } = string.Empty;
}

public class TraitDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class EnvironmentDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class MonsterDetailResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public double ChallengeRating { get; set; }
    public string Size { get; set; } = string.Empty;
    public string? Alignment { get; set; }
    public int HitPoints { get; set; }
    public int ArmorClass { get; set; }
    public int WalkSpeed { get; set; }
    public int? FlySpeed { get; set; }
    public int? SwimSpeed { get; set; }
    public int? BurrowSpeed { get; set; }
    public bool IsSRD { get; set; }
    public string MonsterTypeName { get; set; } = string.Empty;
    public string EditionName { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
    public AbilityScoresDto? AbilityScores { get; set; }
    public List<ActionDto> Actions { get; set; } = new();
    public List<TraitDto> Traits { get; set; } = new();
    public List<EnvironmentDto> Environments { get; set; } = new();
}