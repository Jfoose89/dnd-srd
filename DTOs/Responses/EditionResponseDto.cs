namespace dnd_srd.DTOs.Responses
{
    public class EditionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ReleaseYear { get; set; }
        public string Publisher { get; set; } = string.Empty;
        public string LicenseType { get; set; } = string.Empty;
    }
}
