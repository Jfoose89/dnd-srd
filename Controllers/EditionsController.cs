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
    public class EditionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EditionsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<EditionResponseDto>>> GetEditions()
        {
            var editions = await _context.Editions.ToListAsync();
            return Ok(editions.Select(e => new EditionResponseDto
            {
                Id = e.Id,
                Name = e.Name,
                ReleaseYear = e.ReleaseYear,
                Publisher = e.Publisher,
                LicenseType = e.LicenseType
            }));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EditionResponseDto>> GetEdition(int id)
        {
            var edition = await _context.Editions.FindAsync(id);
            if (edition == null) return NotFound();

            return Ok(new EditionResponseDto
            {
                Id = edition.Id,
                Name = edition.Name,
                ReleaseYear = edition.ReleaseYear,
                Publisher = edition.Publisher,
                LicenseType = edition.LicenseType
            });
        }

        [HttpPost]
        public async Task<ActionResult<EditionResponseDto>> CreateEdition(
            [FromBody] EditionRequestDto dto)
        {
            var edition = new Edition
            {
                Name = dto.Name,
                ReleaseYear = dto.ReleaseYear,
                Publisher = dto.Publisher,
                LicenseType = dto.LicenseType
            };

            _context.Editions.Add(edition);
            await _context.SaveChangesAsync();

            var result = new EditionResponseDto
            {
                Id = edition.Id,
                Name = edition.Name,
                ReleaseYear = edition.ReleaseYear,
                Publisher = edition.Publisher,
                LicenseType = edition.LicenseType
            };

            return CreatedAtAction(nameof(GetEdition), new { id = edition.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<EditionResponseDto>> UpdateEdition(
            int id, [FromBody] EditionRequestDto dto)
        {
            var edition = await _context.Editions.FindAsync(id);
            if (edition == null) return NotFound();

            edition.Name = dto.Name;
            edition.ReleaseYear = dto.ReleaseYear;
            edition.Publisher = dto.Publisher;
            edition.LicenseType = dto.LicenseType;

            await _context.SaveChangesAsync();

            return Ok(new EditionResponseDto
            {
                Id = edition.Id,
                Name = edition.Name,
                ReleaseYear = edition.ReleaseYear,
                Publisher = edition.Publisher,
                LicenseType = edition.LicenseType
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEdition(int id)
        {
            var edition = await _context.Editions.FindAsync(id);
            if (edition == null) return NotFound();

            _context.Editions.Remove(edition);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}