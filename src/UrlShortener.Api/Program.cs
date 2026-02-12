using Microsoft.EntityFrameworkCore;
using Serilog;
using StackExchange.Redis;
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
builder.Services.AddHealthChecks();
builder.Services.AddScoped<IShortUrlRepository, ShortUrlRepository>();
builder.Services.AddScoped<IUrlService, UrlService>();

// EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(configuration!);
});


builder.Services.AddScoped<RedisCacheService>();
builder.Services.AddScoped<IUrlRedirectService, UrlRedirectService>();
builder.Services.AddScoped<IClickEventPublisher, RedisClickEventPublisher>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
