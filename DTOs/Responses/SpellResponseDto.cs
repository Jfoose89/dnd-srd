namespace dnd_srd.DTOs.Responses
{
    public class SpellSummaryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Level { get; set; }
        public string School { get; set; } = string.Empty;
        public bool Concentration { get; set; }
        public bool Ritual { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string EditionName { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
    }

    public class SpellDetailResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Level { get; set; }
        public string School { get; set; } = string.Empty;
        public string CastingTime { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public bool Concentration { get; set; }
        public bool Ritual { get; set; }
        public string Components { get; set; } = string.Empty;
        public bool IsSRD { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string EditionName { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
    }
}
