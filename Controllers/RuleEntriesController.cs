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
    public class RuleEntriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RuleEntriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RuleEntryResponseDto>>> GetRuleEntries(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? editionId = null,
            [FromQuery] string? category = null,
            [FromQuery] string? name = null)
        {
            var query = _context.RuleEntries
                .Include(r => r.Edition)
                .AsQueryable();

            if (editionId.HasValue)
                query = query.Where(r => r.EditionId == editionId.Value);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(r => r.Category.ToLower() == category.ToLower());

            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.Name.ToLower().Contains(name.ToLower()));

            var totalCount = await query.CountAsync();
            var entries = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<RuleEntryResponseDto>
            {
                Data = entries.Select(r => new RuleEntryResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Category = r.Category,
                    Description = r.Description,
                    Prerequisite = r.Prerequisite,
                    Cost = r.Cost,
                    Weight = r.Weight,
                    DamageDice = r.DamageDice,
                    DamageType = r.DamageType,
                    IsSRD = r.IsSRD,
                    EditionName = r.Edition.Name,
                    SourceUrl = $"/api/rules/{r.Id}"
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RuleEntryResponseDto>> GetRuleEntry(int id)
        {
            var entry = await _context.RuleEntries
                .Include(r => r.Edition)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (entry == null) return NotFound();

            return Ok(new RuleEntryResponseDto
            {
                Id = entry.Id,
                Name = entry.Name,
                Category = entry.Category,
                Description = entry.Description,
                Prerequisite = entry.Prerequisite,
                Cost = entry.Cost,
                Weight = entry.Weight,
                DamageDice = entry.DamageDice,
                DamageType = entry.DamageType,
                IsSRD = entry.IsSRD,
                EditionName = entry.Edition.Name,
                SourceUrl = $"/api/rules/{entry.Id}"
            });
        }

        [HttpPost]
        public async Task<ActionResult<RuleEntryResponseDto>> CreateRuleEntry(
            [FromBody] RuleEntryRequestDto dto)
        {
            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            var entry = new RuleEntry
            {
                Name = dto.Name,
                Category = dto.Category,
                Description = dto.Description,
                Prerequisite = dto.Prerequisite,
                Cost = dto.Cost,
                Weight = dto.Weight,
                DamageDice = dto.DamageDice,
                DamageType = dto.DamageType,
                EditionId = dto.EditionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.RuleEntries.Add(entry);
            await _context.SaveChangesAsync();

            var result = new RuleEntryResponseDto
            {
                Id = entry.Id,
                Name = entry.Name,
                Category = entry.Category,
                Description = entry.Description,
                Prerequisite = entry.Prerequisite,
                Cost = entry.Cost,
                Weight = entry.Weight,
                DamageDice = entry.DamageDice,
                DamageType = entry.DamageType,
                IsSRD = entry.IsSRD,
                EditionName = edition.Name,
                SourceUrl = $"/api/rules/{entry.Id}"
            };

            return CreatedAtAction(nameof(GetRuleEntry), new { id = entry.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RuleEntryResponseDto>> UpdateRuleEntry(
            int id, [FromBody] RuleEntryRequestDto dto)
        {
            var entry = await _context.RuleEntries.FindAsync(id);
            if (entry == null) return NotFound();

            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            entry.Name = dto.Name;
            entry.Category = dto.Category;
            entry.Description = dto.Description;
            entry.Prerequisite = dto.Prerequisite;
            entry.Cost = dto.Cost;
            entry.Weight = dto.Weight;
            entry.DamageDice = dto.DamageDice;
            entry.DamageType = dto.DamageType;
            entry.EditionId = dto.EditionId;

            await _context.SaveChangesAsync();

            return Ok(new RuleEntryResponseDto
            {
                Id = entry.Id,
                Name = entry.Name,
                Category = entry.Category,
                Description = entry.Description,
                Prerequisite = entry.Prerequisite,
                Cost = entry.Cost,
                Weight = entry.Weight,
                DamageDice = entry.DamageDice,
                DamageType = entry.DamageType,
                IsSRD = entry.IsSRD,
                EditionName = edition.Name,
                SourceUrl = $"/api/rules/{entry.Id}"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuleEntry(int id)
        {
            var entry = await _context.RuleEntries.FindAsync(id);
            if (entry == null) return NotFound();

            _context.RuleEntries.Remove(entry);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
