using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using UrlShortener.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")!
    )
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

builder.Services.AddHostedService<Worker>();

builder.Build().Run();
