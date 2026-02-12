namespace UrlShortener.Infrastructure.Messaging;

/// <summary>
/// Immutable event representing a single redirect click.
/// This is what we enqueue to Redis.
/// </summary>
public record ClickEvent(
    Guid ShortUrlId,
    DateTimeOffset ClickedAt
);
