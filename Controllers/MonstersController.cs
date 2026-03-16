using dnd_srd.Data;
using dnd_srd.DTOs.Requests;
using dnd_srd.DTOs.Responses;
using dnd_srd.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;

namespace dnd_srd.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("FixedWindow")]
public class MonstersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMemoryCache _cache;
    private const string MonstersCacheKey = "monsters_list";
    private const string MonstersCacheVersion = "monsters_version";

    public MonstersController(AppDbContext context, IMemoryCache cache)
    {
        _context = context;
        _cache = cache;
    }

    // GET /api/monsters
    [HttpGet]
    public async Task<ActionResult<PagedResult<MonsterSummaryResponseDto>>> GetMonsters(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? editionId = null,
        [FromQuery] int? monsterTypeId = null,
        [FromQuery] string? size = null,
        [FromQuery] double? minCR = null,
        [FromQuery] double? maxCR = null,
        [FromQuery] string? name = null)
    {
        var version = _cache.GetOrCreate(MonstersCacheVersion, entry => 0);
        var cacheKey = $"{MonstersCacheKey}_v{version}_{name}_{size}_{minCR}_{maxCR}_{page}_{pageSize}_{editionId}_{monsterTypeId}";

        if (_cache.TryGetValue(cacheKey, out PagedResult<MonsterSummaryResponseDto>? cachedResult) && cachedResult != null)
            return Ok(cachedResult);

        var query = _context.Monsters
            .Include(m => m.Edition)
            .Include(m => m.MonsterType)
            .AsQueryable();

        if (editionId.HasValue)
            query = query.Where(m => m.EditionId == editionId.Value);

        if (monsterTypeId.HasValue)
            query = query.Where(m => m.MonsterTypeId == monsterTypeId.Value);

        if (!string.IsNullOrEmpty(size))
            query = query.Where(m => m.Size.ToLower() == size.ToLower());

        if (minCR.HasValue)
            query = query.Where(m => m.ChallengeRating >= minCR.Value);

        if (maxCR.HasValue)
            query = query.Where(m => m.ChallengeRating <= maxCR.Value);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(m => m.Name.ToLower().Contains(name.ToLower()));

        var totalCount = await query.CountAsync();

        var monsters = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var result = new PagedResult<MonsterSummaryResponseDto>
        {
            Data = monsters.Select(m => new MonsterSummaryResponseDto
            {
                Id = m.Id,
                Name = m.Name,
                ChallengeRating = m.ChallengeRating,
                Size = m.Size,
                Alignment = m.Alignment,
                MonsterTypeName = m.MonsterType.Name,
                EditionName = m.Edition.Name,
                SourceUrl = $"/api/monsters/{m.Id}"
            }).ToList(),
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

        _cache.Set(cacheKey, result, cacheOptions);

        return Ok(result);
    }

    // GET /api/monsters/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<MonsterDetailResponseDto>> GetMonster(int id)
    {
        var monster = await _context.Monsters
            .Include(m => m.Edition)
            .Include(m => m.MonsterType)
            .Include(m => m.AbilityScores)
            .Include(m => m.Actions)
            .Include(m => m.Traits)
            .Include(m => m.Environments)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (monster == null)
            return NotFound();

        var result = new MonsterDetailResponseDto
        {
            Id = monster.Id,
            Name = monster.Name,
            Description = monster.Description,
            ChallengeRating = monster.ChallengeRating,
            Size = monster.Size,
            Alignment = monster.Alignment,
            HitPoints = monster.HitPoints,
            ArmorClass = monster.ArmorClass,
            WalkSpeed = monster.WalkSpeed,
            FlySpeed = monster.FlySpeed,
            SwimSpeed = monster.SwimSpeed,
            BurrowSpeed = monster.BurrowSpeed,
            IsSRD = monster.IsSRD,
            MonsterTypeName = monster.MonsterType.Name,
            EditionName = monster.Edition.Name,
            SourceUrl = $"/api/monsters/{monster.Id}",
            AbilityScores = monster.AbilityScores == null ? null : new AbilityScoresDto
            {
                Strength = monster.AbilityScores.Strength,
                Dexterity = monster.AbilityScores.Dexterity,
                Constitution = monster.AbilityScores.Constitution,
                Intelligence = monster.AbilityScores.Intelligence,
                Wisdom = monster.AbilityScores.Wisdom,
                Charisma = monster.AbilityScores.Charisma,
                StrengthModifier = monster.AbilityScores.StrengthModifier,
                DexterityModifier = monster.AbilityScores.DexterityModifier,
                ConstitutionModifier = monster.AbilityScores.ConstitutionModifier,
                IntelligenceModifier = monster.AbilityScores.IntelligenceModifier,
                WisdomModifier = monster.AbilityScores.WisdomModifier,
                CharismaModifier = monster.AbilityScores.CharismaModifier
            },
            Actions = monster.Actions.Select(a => new ActionDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                AttackBonus = a.AttackBonus,
                DamageDice = a.DamageDice,
                DamageType = a.DamageType,
                ActionType = a.ActionType
            }).ToList(),
            Traits = monster.Traits.Select(t => new TraitDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description ?? string.Empty
            }).ToList(),
            Environments = monster.Environments.Select(e => new EnvironmentDto
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description
            }).ToList()
        };

        return Ok(result);
    }

    // POST /api/monsters
    [HttpPost]
    public async Task<ActionResult<MonsterDetailResponseDto>> CreateMonster(
        [FromBody] MonsterRequestDto dto)
    {
        var edition = await _context.Editions.FindAsync(dto.EditionId);
        if (edition == null)
            return BadRequest("Invalid EditionId — edition not found");

        var monsterType = await _context.MonsterTypes.FindAsync(dto.MonsterTypeId);
        if (monsterType == null)
            return BadRequest("Invalid MonsterTypeId — monster type not found");

        var monster = new Monster
        {
            Name = dto.Name,
            Description = dto.Description,
            HitPoints = dto.HitPoints,
            ArmorClass = dto.ArmorClass,
            WalkSpeed = dto.WalkSpeed,
            FlySpeed = dto.FlySpeed,
            SwimSpeed = dto.SwimSpeed,
            BurrowSpeed = dto.BurrowSpeed,
            ChallengeRating = dto.ChallengeRating,
            Size = dto.Size,
            Alignment = dto.Alignment,
            EditionId = dto.EditionId,
            MonsterTypeId = dto.MonsterTypeId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Monsters.Add(monster);
        await _context.SaveChangesAsync();
        InvalidateMonstersCache();

        return CreatedAtAction(nameof(GetMonster), new { id = monster.Id },
            new MonsterDetailResponseDto
            {
                Id = monster.Id,
                Name = monster.Name,
                Description = monster.Description,
                ChallengeRating = monster.ChallengeRating,
                Size = monster.Size,
                Alignment = monster.Alignment,
                HitPoints = monster.HitPoints,
                ArmorClass = monster.ArmorClass,
                WalkSpeed = monster.WalkSpeed,
                FlySpeed = monster.FlySpeed,
                SwimSpeed = monster.SwimSpeed,
                BurrowSpeed = monster.BurrowSpeed,
                IsSRD = monster.IsSRD,
                MonsterTypeName = monsterType.Name,
                EditionName = edition.Name,
                SourceUrl = $"/api/monsters/{monster.Id}"
            });
    }

    // PUT /api/monsters/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<MonsterDetailResponseDto>> UpdateMonster(
        int id, [FromBody] MonsterRequestDto dto)
    {
        var monster = await _context.Monsters.FindAsync(id);
        if (monster == null)
            return NotFound();

        var edition = await _context.Editions.FindAsync(dto.EditionId);
        if (edition == null)
            return BadRequest("Invalid EditionId — edition not found");

        var monsterType = await _context.MonsterTypes.FindAsync(dto.MonsterTypeId);
        if (monsterType == null)
            return BadRequest("Invalid MonsterTypeId — monster type not found");

        monster.Name = dto.Name;
        monster.Description = dto.Description;
        monster.HitPoints = dto.HitPoints;
        monster.ArmorClass = dto.ArmorClass;
        monster.WalkSpeed = dto.WalkSpeed;
        monster.FlySpeed = dto.FlySpeed;
        monster.SwimSpeed = dto.SwimSpeed;
        monster.BurrowSpeed = dto.BurrowSpeed;
        monster.ChallengeRating = dto.ChallengeRating;
        monster.Size = dto.Size;
        monster.Alignment = dto.Alignment;
        monster.EditionId = dto.EditionId;
        monster.MonsterTypeId = dto.MonsterTypeId;
        monster.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        InvalidateMonstersCache();

        return Ok(new MonsterDetailResponseDto
        {
            Id = monster.Id,
            Name = monster.Name,
            Description = monster.Description,
            ChallengeRating = monster.ChallengeRating,
            Size = monster.Size,
            Alignment = monster.Alignment,
            HitPoints = monster.HitPoints,
            ArmorClass = monster.ArmorClass,
            WalkSpeed = monster.WalkSpeed,
            FlySpeed = monster.FlySpeed,
            SwimSpeed = monster.SwimSpeed,
            BurrowSpeed = monster.BurrowSpeed,
            IsSRD = monster.IsSRD,
            MonsterTypeName = monsterType.Name,
            EditionName = edition.Name,
            SourceUrl = $"/api/monsters/{monster.Id}"
        });
    }

    // DELETE /api/monsters/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMonster(int id)
    {
        var monster = await _context.Monsters.FindAsync(id);
        if (monster == null)
            return NotFound();

        _context.Monsters.Remove(monster);
        await _context.SaveChangesAsync();
        InvalidateMonstersCache();

        return NoContent();
    }

    private void InvalidateMonstersCache()
    {
        var version = _cache.GetOrCreate(MonstersCacheVersion, entry => 0);
        _cache.Set(MonstersCacheVersion, version + 1);
    }
}