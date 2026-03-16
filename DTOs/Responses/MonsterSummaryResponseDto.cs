namespace dnd_srd.DTOs.Responses;

public class MonsterSummaryResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double ChallengeRating { get; set; }
    public string Size { get; set; } = string.Empty;
    public string? Alignment { get; set; }
    public string MonsterTypeName { get; set; } = string.Empty;
    public string EditionName { get; set; } = string.Empty;
    public string SourceUrl { get; set; } = string.Empty;
}