using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Caching;
using UrlShortener.Infrastructure.Messaging;

namespace UrlShortener.Infrastructure.Services;

public class UrlRedirectService : IUrlRedirectService
{
    private readonly RedisCacheService _cache;
    private readonly IShortUrlRepository _repository;
    private readonly IClickEventPublisher _publisher;

    public UrlRedirectService(
        RedisCacheService cache,
        IShortUrlRepository repository,
        IClickEventPublisher publisher)
    {
        _cache = cache;
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<string?> ResolveAsync(
     string shortCode,
     CancellationToken cancellationToken = default)
    {
        // 1️⃣ Try cache
        var cached = await _cache.GetAsync<ShortUrl>(shortCode);
        if (cached is not null)
        {
            await _publisher.PublishAsync(
                new ClickEvent(cached.Id, DateTimeOffset.UtcNow)
            );

            return cached.LongUrl;
        }

        // 2️⃣ DB fallback
        var entity = await _repository.GetByShortCodeAsync(
            shortCode,
            cancellationToken
        );

        if (entity is null)
            return null;

        // 3️⃣ Cache with TTL
        await _cache.SetAsync(
            shortCode,
            entity,
            TimeSpan.FromHours(1)
        );

        // 4️⃣ Async analytics event
        await _publisher.PublishAsync(
            new ClickEvent(entity.Id, DateTimeOffset.UtcNow)
        );

        return entity.LongUrl;
    }

}
