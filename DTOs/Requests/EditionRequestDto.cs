using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests
{
    public class EditionRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1970, 2100)]
        public int ReleaseYear { get; set; }

        [Required]
        [StringLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LicenseType { get; set; } = string.Empty;
    }
}
