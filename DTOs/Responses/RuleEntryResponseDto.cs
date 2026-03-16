namespace dnd_srd.DTOs.Responses
{
    public class RuleEntryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Prerequisite { get; set; }
        public string? Cost { get; set; }
        public double? Weight { get; set; }
        public string? DamageDice { get; set; }
        public string? DamageType { get; set; }
        public bool IsSRD { get; set; }
        public string EditionName { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
    }
}
