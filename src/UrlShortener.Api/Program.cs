using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Serilog;
using StackExchange.Redis;
using System.Threading.RateLimiting;
using UrlShortener.Application.Interfaces;
using UrlShortener.Application.Services;
using UrlShortener.Infrastructure.Caching;
using UrlShortener.Infrastructure.Messaging;
using UrlShortener.Infrastructure.Persistence;
using UrlShortener.Infrastructure.Persistence.Repositories;
using UrlShortener.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration)
      .WriteTo.Console());

builder.Services.AddControllers();

builder.Services.AddHealthChecks()
    .AddNpgSql(
        builder.Configuration.GetConnectionString("Postgres")!,
        name: "postgres"
    )
    .AddRedis(
        builder.Configuration.GetConnectionString("Redis")!,
        name: "redis"
    );

// Application + Infrastructure
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IUrlAnalyticsRepository, UrlAnalyticsRepository>();
builder.Services.AddScoped<IUrlService, UrlService>();
builder.Services.AddScoped<IUrlAnalyticsService, UrlAnalyticsService>();

// Rate limiting (global)
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(_ =>
        RateLimitPartition.GetFixedWindowLimiter(
            "global",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 50,
                Window = TimeSpan.FromSeconds(10)
            }));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "URL Shortener API",
        Version = "v1"
    });
});


// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var options = ConfigurationOptions.Parse(
        builder.Configuration.GetConnectionString("Redis")!
    );

    // Important for container / startup resilience
    options.AbortOnConnectFail = false;

    return ConnectionMultiplexer.Connect(options);
});

builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddScoped<IUrlRedirectService, UrlRedirectService>();
builder.Services.AddScoped<IClickEventPublisher, RedisClickEventPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseRateLimiter();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
