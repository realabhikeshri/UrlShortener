using StackExchange.Redis;
using System.Text.Json;

namespace UrlShortener.Infrastructure.Messaging;

public class RedisClickEventPublisher : IClickEventPublisher
{
    private const string QueueName = "click-events";
    private readonly IDatabase _db;

    public RedisClickEventPublisher(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task PublishAsync(ClickEvent clickEvent)
    {
        var payload = JsonSerializer.Serialize(clickEvent);
        await _db.ListRightPushAsync(QueueName, payload);
    }
}
