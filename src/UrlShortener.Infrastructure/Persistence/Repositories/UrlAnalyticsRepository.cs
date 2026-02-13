using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Infrastructure.Persistence;

namespace UrlShortener.Infrastructure.Persistence.Repositories;

public sealed class UrlAnalyticsRepository : IUrlAnalyticsRepository
{
    private readonly AppDbContext _db;

    public UrlAnalyticsRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<long> GetClickCountAsync(
        Guid shortUrlId,
        CancellationToken cancellationToken)
    {
        return await _db.UrlAnalytics
            .Where(a => a.ShortUrlId == shortUrlId)
            .Select(a => a.ClickCount)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
