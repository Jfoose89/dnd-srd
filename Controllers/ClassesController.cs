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
    public class ClassesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClassesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<ClassResponseDto>>> GetClasses(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? editionId = null,
            [FromQuery] string? hitDie = null,
            [FromQuery] int? parentClassId = null,
            [FromQuery] string? name = null)
        {
            var query = _context.Classes
                .Include(c => c.Edition)
                .Include(c => c.ParentClass)
                .Include(c => c.Subclasses)
                .AsQueryable();

            if (editionId.HasValue)
                query = query.Where(c => c.EditionId == editionId.Value);

            if (!string.IsNullOrEmpty(hitDie))
                query = query.Where(c => c.HitDie.ToLower() == hitDie.ToLower());

            if (parentClassId.HasValue)
                query = query.Where(c => c.ParentClassId == parentClassId.Value);

            if (!string.IsNullOrEmpty(name))
                query = query.Where(c => c.Name.ToLower().Contains(name.ToLower()));

            var totalCount = await query.CountAsync();
            var classes = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new PagedResult<ClassResponseDto>
            {
                Data = classes.Select(c => new ClassResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    HitDie = c.HitDie,
                    PrimaryAbility = c.PrimaryAbility,
                    SavingThrows = c.SavingThrows,
                    ArmorProficiencies = c.ArmorProficiencies,
                    WeaponProficiencies = c.WeaponProficiencies,
                    IsSRD = c.IsSRD,
                    EditionName = c.Edition.Name,
                    ParentClassName = c.ParentClass?.Name,
                    Subclasses = c.Subclasses.Select(s => s.Name).ToList(),
                    SourceUrl = $"/api/classes/{c.Id}"
                }).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClassResponseDto>> GetClass(int id)
        {
            var c = await _context.Classes
                .Include(c => c.Edition)
                .Include(c => c.ParentClass)
                .Include(c => c.Subclasses)
                .Include(c => c.Spells)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (c == null) return NotFound();

            return Ok(new ClassResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                HitDie = c.HitDie,
                PrimaryAbility = c.PrimaryAbility,
                SavingThrows = c.SavingThrows,
                ArmorProficiencies = c.ArmorProficiencies,
                WeaponProficiencies = c.WeaponProficiencies,
                IsSRD = c.IsSRD,
                EditionName = c.Edition.Name,
                ParentClassName = c.ParentClass?.Name,
                Subclasses = c.Subclasses.Select(s => s.Name).ToList(),
                Spells = c.Spells.Select(s => s.Name).ToList(),
                SourceUrl = $"/api/classes/{c.Id}"
            });
        }

        [HttpPost]
        public async Task<ActionResult<ClassResponseDto>> CreateClass(
            [FromBody] ClassRequestDto dto)
        {
            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            if (dto.ParentClassId.HasValue)
            {
                var parent = await _context.Classes.FindAsync(dto.ParentClassId.Value);
                if (parent == null)
                    return BadRequest("Invalid ParentClassId — parent class not found");
            }

            var newClass = new Class
            {
                Name = dto.Name,
                Description = dto.Description,
                HitDie = dto.HitDie,
                PrimaryAbility = dto.PrimaryAbility,
                SavingThrows = dto.SavingThrows,
                ArmorProficiencies = dto.ArmorProficiencies,
                WeaponProficiencies = dto.WeaponProficiencies,
                EditionId = dto.EditionId,
                ParentClassId = dto.ParentClassId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClass), new { id = newClass.Id }, new ClassResponseDto
            {
                Id = newClass.Id,
                Name = newClass.Name,
                Description = newClass.Description,
                HitDie = newClass.HitDie,
                PrimaryAbility = newClass.PrimaryAbility,
                SavingThrows = newClass.SavingThrows,
                ArmorProficiencies = newClass.ArmorProficiencies,
                WeaponProficiencies = newClass.WeaponProficiencies,
                IsSRD = newClass.IsSRD,
                EditionName = edition.Name,
                ParentClassName = newClass.ParentClass?.Name,
                Subclasses = new List<string>(),
                SourceUrl = $"/api/classes/{newClass.Id}"
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ClassResponseDto>> UpdateClass(
            int id, [FromBody] ClassRequestDto dto)
        {
            var c = await _context.Classes
                .Include(c => c.ParentClass)
                .Include(c => c.Subclasses)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (c == null) return NotFound();

            var edition = await _context.Editions.FindAsync(dto.EditionId);
            if (edition == null)
                return BadRequest("Invalid EditionId — edition not found");

            c.Name = dto.Name;
            c.Description = dto.Description;
            c.HitDie = dto.HitDie;
            c.PrimaryAbility = dto.PrimaryAbility;
            c.SavingThrows = dto.SavingThrows;
            c.ArmorProficiencies = dto.ArmorProficiencies;
            c.WeaponProficiencies = dto.WeaponProficiencies;
            c.EditionId = dto.EditionId;
            c.ParentClassId = dto.ParentClassId;

            await _context.SaveChangesAsync();

            return Ok(new ClassResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                HitDie = c.HitDie,
                PrimaryAbility = c.PrimaryAbility,
                SavingThrows = c.SavingThrows,
                ArmorProficiencies = c.ArmorProficiencies,
                WeaponProficiencies = c.WeaponProficiencies,
                IsSRD = c.IsSRD,
                EditionName = edition.Name,
                ParentClassName = c.ParentClass?.Name,
                Subclasses = c.Subclasses.Select(s => s.Name).ToList(),
                SourceUrl = $"/api/classes/{c.Id}"
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClass(int id)
        {
            var c = await _context.Classes.FindAsync(id);
            if (c == null) return NotFound();

            _context.Classes.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
