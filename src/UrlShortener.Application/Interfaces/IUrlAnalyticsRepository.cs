namespace UrlShortener.Application.Interfaces;

public interface IUrlAnalyticsRepository
{
    Task<long> GetClickCountAsync(
        Guid shortUrlId,
        CancellationToken cancellationToken
    );
}
