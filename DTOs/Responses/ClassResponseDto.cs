namespace dnd_srd.DTOs.Responses
{

    public class ClassResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string HitDie { get; set; } = string.Empty;
        public string PrimaryAbility { get; set; } = string.Empty;
        public string SavingThrows { get; set; } = string.Empty;
        public string ArmorProficiencies { get; set; } = string.Empty;
        public string WeaponProficiencies { get; set; } = string.Empty;
        public bool IsSRD { get; set; }
        public string EditionName { get; set; } = string.Empty;
        public string? ParentClassName { get; set; }
        public List<string> Subclasses { get; set; } = new();
        public string SourceUrl { get; set; } = string.Empty;
    }
}