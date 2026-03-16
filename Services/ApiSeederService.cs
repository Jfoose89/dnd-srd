using dnd_srd.Data;
using dnd_srd.Models;
using System.Text.Json;

namespace dnd_srd.Services;

public class ApiSeederService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ApiSeederService> _logger;

    public ApiSeederService(
        IHttpClientFactory httpClientFactory,
        IServiceScopeFactory scopeFactory,
        ILogger<ApiSeederService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var edition5e = context.Editions.FirstOrDefault(e => e.Name == "D&D 5th Edition");
        if (edition5e == null)
        {
            _logger.LogWarning("D&D 5th Edition not found in database. Skipping API seeding.");
            return;
        }

        var defaultMonsterType = context.MonsterTypes.FirstOrDefault(mt => mt.Name == "Beast");
        if (defaultMonsterType == null)
        {
            _logger.LogWarning("Default monster type not found. Skipping API seeding");
            return;
        }

        await SeedMonstersAsync(context, edition5e, defaultMonsterType);
        await SeedSpellsAsync(context, edition5e);
    }

    private async Task SeedMonstersAsync(AppDbContext context, Edition edition5e, MonsterType defaultType)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Open5eClient");
            var response = await client.GetAsync("https://api.open5e.com/v1/monsters/?limit=50&document__slug=wotc-srd");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch monsters from Open5e: {Status}", response.StatusCode);
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");

            int added = 0;
            foreach (var item in results.EnumerateArray())
            {
                var name = item.GetProperty("name").GetString() ?? string.Empty;

                if (string.IsNullOrEmpty(name) || context.Monsters.Any(m => m.Name == name))
                    continue;

                var monsterTypeName = item.TryGetProperty("type", out var typeEl)
                    ? typeEl.GetString() ?? "Beast"
                    : "Beast";

                var monsterType = context.MonsterTypes
                    .FirstOrDefault(mt => mt.Name.ToLower() == monsterTypeName.ToLower())
                    ?? defaultType;

                var hitPointsStr = item.TryGetProperty("hit_points", out var hpEl)
                    ? hpEl.GetInt32() : 1;

                var armorClass = item.TryGetProperty("armor_class", out var acEl)
                    ? acEl.GetInt32() : 10;

                var crRaw = item.TryGetProperty("challenge_rating", out var crEl)
                    ? crEl.GetString() ?? "0" : "0";
                var challengeRating = ParseChallengeRating(crRaw);

                var size = item.TryGetProperty("size", out var sizeEl)
                    ? sizeEl.GetString() ?? "Medium" : "Medium";

                var alignment = item.TryGetProperty("alignment", out var alignEl)
                    ? alignEl.GetString() ?? "Unaligned" : "Unaligned";

                var desc = item.TryGetProperty("desc", out var descEl)
                    ? descEl.GetString() ?? string.Empty : string.Empty;

                var monster = new Monster
                {
                    Name = name,
                    Description = desc,
                    HitPoints = hitPointsStr,
                    ArmorClass = armorClass,
                    WalkSpeed = 30,
                    ChallengeRating = challengeRating,
                    Size = size,
                    Alignment = alignment,
                    IsSRD = true,
                    Edition = edition5e,
                    MonsterType = monsterType
                };

                context.Monsters.Add(monster);
                added++;
            }

            await context.SaveChangesAsync();
            _logger.LogInformation("API seeder added {Count} monsters from Open5e.", added);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding monsters from Open5e.");
        }
    }

    private async Task SeedSpellsAsync(AppDbContext context, Edition edition5e)
    {
        try
        {
            var client = _httpClientFactory.CreateClient("Open5eClient");
            var response = await client.GetAsync("https://api.open5e.com/v1/spells/?limit=50&document__slug=wotc-srd");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to fetch spells from Open5e: {Status}", response.StatusCode);
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");

            var wizardClass = context.Classes.FirstOrDefault(c => c.Name == "Wizard");

            int added = 0;
            foreach (var item in results.EnumerateArray())
            {
                var name = item.GetProperty("name").GetString() ?? string.Empty;

                if (string.IsNullOrEmpty(name) || context.Spells.Any(s => s.Name == name))
                    continue;

                var level = item.TryGetProperty("spell_level", out var lvlEl)
                    ? lvlEl.GetInt32() : 0;

                var school = item.TryGetProperty("school", out var schoolEl)
                    ? schoolEl.GetString() ?? "Evocation" : "Evocation";

                var castingTime = item.TryGetProperty("casting_time", out var ctEl)
                    ? ctEl.GetString() ?? "1 action" : "1 action";

                var range = item.TryGetProperty("range", out var rangeEl)
                    ? rangeEl.GetString() ?? "Self" : "Self";

                var duration = item.TryGetProperty("duration", out var durEl)
                    ? durEl.GetString() ?? "Instantaneous" : "Instantaneous";

                var components = item.TryGetProperty("components", out var compEl)
                    ? compEl.GetString() ?? "V" : "V";

                var concentration = item.TryGetProperty("concentration", out var concEl)
                    && concEl.GetString()?.ToLower() == "yes";

                var ritual = item.TryGetProperty("ritual", out var ritEl)
                    && ritEl.GetString()?.ToLower() == "yes";

                var desc = item.TryGetProperty("desc", out var descEl)
                    ? descEl.GetString() ?? string.Empty : string.Empty;

                var spell = new Spell
                {
                    Name = name,
                    Description = desc,
                    Level = level,
                    School = school,
                    CastingTime = castingTime,
                    Range = range,
                    Duration = duration,
                    Components = components,
                    Concentration = concentration,
                    Ritual = ritual,
                    IsSRD = true,
                    Edition = edition5e,
                    Class = wizardClass!
                };

                context.Spells.Add(spell);
                added++;
            }

            await context.SaveChangesAsync();
            _logger.LogInformation("API seeder added {Count} spells from Open5e.", added);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding spells from Open5e.");
        }
    }

    private static double ParseChallengeRating(string cr) => cr switch
    {
        "1/8" => 0.125,
        "1/4" => 0.25,
        "1/2" => 0.5,
        _ => double.TryParse(cr, out var result) ? result : 0
    };
}