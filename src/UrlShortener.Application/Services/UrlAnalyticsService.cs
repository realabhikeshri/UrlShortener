using UrlShortener.Application.Interfaces;

namespace UrlShortener.Application.Services;

public sealed class UrlAnalyticsService : IUrlAnalyticsService
{
    private readonly IShortUrlRepository _shortUrlRepository;
    private readonly IUrlAnalyticsRepository _analyticsRepository;

    public UrlAnalyticsService(
        IShortUrlRepository shortUrlRepository,
        IUrlAnalyticsRepository analyticsRepository)
    {
        _shortUrlRepository = shortUrlRepository;
        _analyticsRepository = analyticsRepository;
    }

    public async Task<long> GetAsync(
        string shortCode,
        CancellationToken cancellationToken)
    {
        var shortUrl = await _shortUrlRepository
            .GetByShortCodeAsync(shortCode, cancellationToken);

        if (shortUrl is null)
            return 0;

        return await _analyticsRepository.GetClickCountAsync(
            shortUrl.Id,
            cancellationToken);
    }
}
