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
    public class RacesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RacesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<RaceResponseDto>>> GetRaces(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? editionId = null,
            [FromQuery] string? size = null,
            [FromQuery] string? name = null)
        {
            var query = _context.Races
                .Include(r => r.Edition)
                .AsQueryable();

            if (editionId.HasValue)
                query = query.Where(r => r.EditionId == editionId.Value);

            if (!string.IsNullOrEmpty(size))
                query = query.Where(r => r.Size.ToLower() == size.ToLower());

            if (!string.IsNullOrEmpty(name))
                query = query.Where(r => r.Name.ToLower().Contains(name.ToLower()));

            var totalCount = await query.CountAsync();
            var races = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<RaceResponseDto>
            {
                Data = races.Select(r => new RaceResponseDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Speed = r.Speed,
                    Size = r.Size,
                    AbilityScoreBonus = r.AbilityScoreBonus,
                    Traits = r.Traits,
                    Languages = r.Languages,
                    IsSRD = r.IsSRD,
                    EditionName = r.Edition.Name,
                    SourceUrl = $"/api/races/{r.Id}"
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RaceResponseDto>> GetRace(int id)
        {
            var race = await _context.Races
                .Include(r => r.Edition)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (race == null) return NotFound();

            return Ok(new RaceResponseDto
            {
                Id = race.Id,
                Name = race.Name,
                Description = race.Description,
                Speed = race.Speed,
                Size = race.Size,
                AbilityScoreBonus = race.AbilityScoreBonus,
                Traits = race.Traits,
                Languages = race.Languages,
                IsSRD = race.IsSRD,
                EditionName = race.Edition.Name,
                SourceUrl = $"/api/races/{race.Id}"
            });
        }

        [HttpPost]
        public async Task<ActionResult<RaceResponseDto>> CreateRace(
            [FromBody] RaceRequestDto dto)
        {
            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            var race = new Race
            {
                Name = dto.Name,
                Description = dto.Description,
                Speed = dto.Speed,
                Size = dto.Size,
                AbilityScoreBonus = dto.AbilityScoreBonus,
                Traits = dto.Traits,
                Languages = dto.Languages,
                EditionId = dto.EditionId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Races.Add(race);
            await _context.SaveChangesAsync();

            var result = new RaceResponseDto
            {
                Id = race.Id,
                Name = race.Name,
                Description = race.Description,
                Speed = race.Speed,
                Size = race.Size,
                AbilityScoreBonus = race.AbilityScoreBonus,
                Traits = race.Traits,
                Languages = race.Languages,
                IsSRD = race.IsSRD,
                EditionName = edition.Name,
                SourceUrl = $"/api/races/{race.Id}"
            };

            return CreatedAtAction(nameof(GetRace), new { id = race.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<RaceResponseDto>> UpdateRace(
            int id, [FromBody] RaceRequestDto dto)
        {
            var race = await _context.Races.FindAsync(id);
            if (race == null) return NotFound();

            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            race.Name = dto.Name;
            race.Description = dto.Description;
            race.Speed = dto.Speed;
            race.Size = dto.Size;
            race.AbilityScoreBonus = dto.AbilityScoreBonus;
            race.Traits = dto.Traits;
            race.Languages = dto.Languages;
            race.EditionId = dto.EditionId;

            await _context.SaveChangesAsync();

            return Ok(new RaceResponseDto
            {
                Id = race.Id,
                Name = race.Name,
                Description = race.Description,
                Speed = race.Speed,
                Size = race.Size,
                AbilityScoreBonus = race.AbilityScoreBonus,
                Traits = race.Traits,
                Languages = race.Languages,
                IsSRD = race.IsSRD,
                EditionName = edition.Name,
                SourceUrl = $"/api/races/{race.Id}"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRace(int id)
        {
            var race = await _context.Races.FindAsync(id);
            if (race == null) return NotFound();

            _context.Races.Remove(race);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
