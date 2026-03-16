using dnd_srd.Data;
using dnd_srd.DTOs.Requests;
using dnd_srd.DTOs.Responses;
using dnd_srd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dnd_srd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpellsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SpellsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<SpellSummaryResponseDto>>> GetSpells(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? editionId = null,
            [FromQuery] int? classId = null,
            [FromQuery] int? level = null,
            [FromQuery] string? school = null,
            [FromQuery] bool? concentration = null,
            [FromQuery] bool? ritual = null,
            [FromQuery] string? name = null)
        {
            var query = _context.Spells
                .Include(s => s.Edition)
                .Include(s => s.Class)
                .AsQueryable();

            if (editionId.HasValue)
                query = query.Where(s => s.EditionId == editionId.Value);

            if (classId.HasValue)
                query = query.Where(s => s.ClassId == classId.Value);

            if (level.HasValue)
                query = query.Where(s => s.Level == level.Value);

            if (!string.IsNullOrEmpty(school))
                query = query.Where(s => s.School.ToLower() == school.ToLower());

            if (concentration.HasValue)
                query = query.Where(s => s.Concentration == concentration.Value);

            if (ritual.HasValue)
                query = query.Where(s => s.Ritual == ritual.Value);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(s => s.Name.ToLower().Contains(name.ToLower()));

            var totalCount = await query.CountAsync();
            var spells = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<SpellSummaryResponseDto>
            {
                Data = spells.Select(s => new SpellSummaryResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Level = s.Level,
                    School = s.School,
                    Concentration = s.Concentration,
                    Ritual = s.Ritual,
                    ClassName = s.Class.Name,
                    EditionName = s.Edition.Name,
                    SourceUrl = $"/api/spells/{s.Id}"
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SpellDetailResponseDto>> GetSpell(int id)
        {
            var spell = await _context.Spells
                .Include(s => s.Edition)
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (spell == null) return NotFound();

            return Ok(new SpellDetailResponseDto
            {
                Id = spell.Id,
                Name = spell.Name,
                Description = spell.Description,
                Level = spell.Level,
                School = spell.School,
                CastingTime = spell.CastingTime,
                Range = spell.Range,
                Duration = spell.Duration,
                Concentration = spell.Concentration,
                Ritual = spell.Ritual,
                Components = spell.Components,
                IsSRD = spell.IsSRD,
                ClassName = spell.Class.Name,
                EditionName = spell.Edition.Name,
                SourceUrl = $"/api/spells/{spell.Id}"
            });
        }

        [HttpPost]
        public async Task<ActionResult<SpellDetailResponseDto>> CreateSpell(
            [FromBody] SpellRequestDto dto)
        {
            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            var cls = await _context.Classes.FindAsync(dto.ClassId);
            if (cls == null)
                return BadRequest("Invalid ClassId — class not found");

            var spell = new Spell
            {
                Name = dto.Name,
                Description = dto.Description,
                Level = dto.Level,
                School = dto.School,
                CastingTime = dto.CastingTime,
                Range = dto.Range,
                Duration = dto.Duration,
                Concentration = dto.Concentration,
                Ritual = dto.Ritual,
                Components = dto.Components,
                EditionId = dto.EditionId,
                ClassId = dto.ClassId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Spells.Add(spell);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSpell), new { id = spell.Id }, new SpellDetailResponseDto
            {
                Id = spell.Id,
                Name = spell.Name,
                Description = spell.Description,
                Level = spell.Level,
                School = spell.School,
                CastingTime = spell.CastingTime,
                Range = spell.Range,
                Duration = spell.Duration,
                Concentration = spell.Concentration,
                Ritual = spell.Ritual,
                Components = spell.Components,
                IsSRD = spell.IsSRD,
                ClassName = cls.Name,
                EditionName = edition.Name,
                SourceUrl = $"/api/spells/{spell.Id}"
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SpellDetailResponseDto>> UpdateSpell(
            int id, [FromBody] SpellRequestDto dto)
        {
            var spell = await _context.Spells.FindAsync(id);
            if (spell == null) return NotFound();

            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            var cls = await _context.Classes.FindAsync(dto.ClassId);
            if (cls == null)
                return BadRequest("Invalid ClassId — class not found");

            spell.Name = dto.Name;
            spell.Description = dto.Description;
            spell.Level = dto.Level;
            spell.School = dto.School;
            spell.CastingTime = dto.CastingTime;
            spell.Range = dto.Range;
            spell.Duration = dto.Duration;
            spell.Concentration = dto.Concentration;
            spell.Ritual = dto.Ritual;
            spell.Components = dto.Components;
            spell.EditionId = dto.EditionId;
            spell.ClassId = dto.ClassId;

            await _context.SaveChangesAsync();

            return Ok(new SpellDetailResponseDto
            {
                Id = spell.Id,
                Name = spell.Name,
                Description = spell.Description,
                Level = spell.Level,
                School = spell.School,
                CastingTime = spell.CastingTime,
                Range = spell.Range,
                Duration = spell.Duration,
                Concentration = spell.Concentration,
                Ritual = spell.Ritual,
                Components = spell.Components,
                IsSRD = spell.IsSRD,
                ClassName = cls.Name,
                EditionName = edition.Name,
                SourceUrl = $"/api/spells/{spell.Id}"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpell(int id)
        {
            var spell = await _context.Spells.FindAsync(id);
            if (spell == null) return NotFound();

            _context.Spells.Remove(spell);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
