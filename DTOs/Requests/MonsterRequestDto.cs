using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests;

public class MonsterRequestDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    [Range(1, 9999)]
    public int HitPoints { get; set; }

    [Required]
    [Range(1, 30)]
    public int ArmorClass { get; set; }

    [Required]
    [Range(0, 999)]
    public int WalkSpeed { get; set; }

    [Range(0, 999)]
    public int? FlySpeed { get; set; }

    [Range(0, 999)]
    public int? SwimSpeed { get; set; }

    [Range(0, 999)]
    public int? BurrowSpeed { get; set; }

    [Required]
    [Range(0, 30)]
    public double ChallengeRating { get; set; }

    [Required]
    [StringLength(20)]
    public string Size { get; set; } = string.Empty;

    public string? Alignment { get; set; }

    [Required]
    public int EditionId { get; set; }

    [Required]
    public int MonsterTypeId { get; set; }
}