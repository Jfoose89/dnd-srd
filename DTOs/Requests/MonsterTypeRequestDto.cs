using System.ComponentModel.DataAnnotations;

namespace dnd_srd.DTOs.Requests
{
    public class MonsterTypeRequestDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
