namespace dnd_srd.DTOs.Responses
{
    public class RaceResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Speed { get; set; }
        public string Size { get; set; } = string.Empty;
        public string AbilityScoreBonus { get; set; } = string.Empty;
        public string Traits { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public bool IsSRD { get; set; }
        public string EditionName { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
    }
}
