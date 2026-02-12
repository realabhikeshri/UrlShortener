using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Interfaces;

public interface IShortUrlRepository
{
    /// <summary>
    /// Used during redirect flow (shortCode -> longUrl)
    /// </summary>
    Task<ShortUrl?> GetByShortCodeAsync(
        string shortCode,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Used during creation flow to prevent duplicates
    /// </summary>
    Task<ShortUrl?> GetByLongUrlAsync(
        string longUrl,
        CancellationToken cancellationToken
    );

    /// <summary>
    /// Persists a new short URL
    /// </summary>
    Task AddAsync(
        ShortUrl entity,
        CancellationToken cancellationToken
    );
}
