using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace dnd_srd.Services;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyMiddleware> _logger;
    private const string ApiKeyHeader = "X-Api-Key";

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<ApiKeyMiddleware> logger)
    {
        _next = next;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var method = context.Request.Method;
        var isProtectedMethod = method == HttpMethods.Post
            || method == HttpMethods.Put
            || method == HttpMethods.Delete;

        if (isProtectedMethod)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API key missing.");
                return;
            }

            var validApiKey = _configuration["ApiKeys:InternalApiKey"];

            if (!string.IsNullOrEmpty(validApiKey))
            {
                _logger.LogWarning("ApiKeys:InternalApiKey is not configured.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("API key configuration missing.");
                return;
            }
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync("Invalid API key.");
                return;
            }
        }

        await _next(context);
    }
}