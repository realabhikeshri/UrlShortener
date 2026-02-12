using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Messaging;
using UrlShortener.Infrastructure.Persistence;

public class Worker : BackgroundService
{
    private const string QueueName = "click-events";
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IDatabase _redis;

    public Worker(
        IConnectionMultiplexer redis,
        IServiceScopeFactory scopeFactory)
    {
        _redis = redis.GetDatabase();
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await _redis.ListLeftPopAsync(QueueName);

            if (result.IsNullOrEmpty)
            {
                await Task.Delay(200, stoppingToken);
                continue;
            }

            var click = JsonSerializer.Deserialize<ClickEvent>(result!);
            if (click is null) continue;

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var analytics = await db.UrlAnalytics
    .FirstOrDefaultAsync(x => x.ShortUrlId == click.ShortUrlId);

            if (analytics is null)
            {
                analytics = new UrlAnalytics(click.ShortUrlId);
                db.UrlAnalytics.Add(analytics);
            }
            else
            {
                analytics.Increment();
            }

            await db.SaveChangesAsync(stoppingToken);

        }
    }
}
