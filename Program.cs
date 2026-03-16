using dnd_srd.Services;
using dnd_srd.Data;
using Microsoft.EntityFrameworkCore;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy via extension method
builder.Services.AddCorsPolicies();
builder.Services.AddRateLimitingPolicies();
builder.Services.AddCachingPolicies();

builder.Services.AddHttpClient("Open5eClient", client =>
{
    client.BaseAddress = new Uri("https://api.open5e.com/v1/");
    client.Timeout = TimeSpan.FromSeconds(30);
})
.AddTransientHttpErrorPolicy(policy =>
    policy.WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
        onRetry: (outcome, timespan, attempt, context) =>
        {
            Console.WriteLine($"Open5e retry attempt {attempt} after {timespan.TotalSeconds}s");
        }
    ))
.AddTransientHttpErrorPolicy(policy =>
    policy.CircuitBreakerAsync(
        handledEventsAllowedBeforeBreaking: 5,
        durationOfBreak: TimeSpan.FromSeconds(30)
    ));

// Add DbContext with In-Memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("DndSrdDb"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionMiddleware>();

// Add CORS middleware
app.UseCors("DndSrdPolicy");
app.UseRateLimiter();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseAuthorization();
app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSeeder.Seed(context);
}

// API Seeder
var apiSeeder = new ApiSeederService(
    app.Services.GetRequiredService<IHttpClientFactory>(),
    app.Services.GetRequiredService<IServiceScopeFactory>(),
    app.Services.GetRequiredService<ILogger<ApiSeederService>>()
);
await apiSeeder.SeedAsync();

app.Run();