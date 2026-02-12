using UrlShortener.Application.Dtos;
using UrlShortener.Application.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Services;

namespace UrlShortener.Application.Services;

public class UrlService : IUrlService
{
    private readonly IShortUrlRepository _repository;

    public UrlService(IShortUrlRepository repository)
    {
        _repository = repository;
    }

    public async Task<CreateShortUrlResponse> CreateAsync(
        CreateShortUrlRequest request,
        CancellationToken cancellationToken)
    {
        var normalizedUrl = request.LongUrl.Trim();

        var existing = await _repository
            .GetByLongUrlAsync(normalizedUrl, cancellationToken);

        if (existing != null)
        {
            return new CreateShortUrlResponse(
                existing.ShortCode,
                BuildShortUrl(existing.ShortCode));
        }

        var shortCode = ShortCodeGenerator.Generate(normalizedUrl);

        var entity = new ShortUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = normalizedUrl,
            ShortCode = shortCode,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _repository.AddAsync(entity, cancellationToken);

        return new CreateShortUrlResponse(
            shortCode,
            BuildShortUrl(shortCode));
    }

    private static string BuildShortUrl(string code)
        => $"http://localhost:8080/{code}";
}
