namespace UrlShortener.Application.Interfaces;

public interface IUrlAnalyticsService
{
    Task<long> GetAsync(
        string shortCode,
        CancellationToken cancellationToken = default
    );
}
