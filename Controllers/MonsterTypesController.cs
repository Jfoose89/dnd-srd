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
    public class MonsterTypesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MonsterTypesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<MonsterTypeResponseDto>>> GetMonsterTypes()
        {
            var types = await _context.MonsterTypes.ToListAsync();
            return Ok(types.Select(t => new MonsterTypeResponseDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description!
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MonsterTypeResponseDto>> GetMonsterType(int id)
        {
            var type = await _context.MonsterTypes.FindAsync(id);
            if (type == null) return NotFound();

            return Ok(new MonsterTypeResponseDto
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description!
            });
        }

        [HttpPost]
        public async Task<ActionResult<MonsterTypeResponseDto>> CreateMonsterType(
            [FromBody] MonsterTypeRequestDto dto)
        {
            var type = new MonsterType
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.MonsterTypes.Add(type);
            await _context.SaveChangesAsync();

            var result = new MonsterTypeResponseDto
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description!
            };

            return CreatedAtAction(nameof(GetMonsterType), new { id = type.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MonsterTypeResponseDto>> UpdateMonsterType(
            int id, [FromBody] MonsterTypeRequestDto dto)
        {
            var type = await _context.MonsterTypes.FindAsync(id);
            if (type == null) return NotFound();

            type.Name = dto.Name;
            type.Description = dto.Description;

            await _context.SaveChangesAsync();

            return Ok(new MonsterTypeResponseDto
            {
                Id = type.Id,
                Name = type.Name,
                Description = type.Description!
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonsterType(int id)
        {
            var type = await _context.MonsterTypes.FindAsync(id);
            if (type == null) return NotFound();

            _context.MonsterTypes.Remove(type);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
