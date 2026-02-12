using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Persistence.Repositories;

public class ShortUrlRepository : IShortUrlRepository
{
    private readonly AppDbContext _dbContext;

    public ShortUrlRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShortUrl?> GetByLongUrlAsync(
        string longUrl,
        CancellationToken cancellationToken)
    {
        return await _dbContext.ShortUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.LongUrl == longUrl,
                cancellationToken
            );
    }

    public async Task<ShortUrl?> GetByShortCodeAsync(
        string shortCode,
        CancellationToken cancellationToken)
    {
        return await _dbContext.ShortUrls
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.ShortCode == shortCode,
                cancellationToken
            );
    }

    public async Task AddAsync(
        ShortUrl entity,
        CancellationToken cancellationToken)
    {
        _dbContext.ShortUrls.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
