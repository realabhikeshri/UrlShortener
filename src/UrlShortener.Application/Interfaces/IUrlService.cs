using UrlShortener.Application.Dtos;

namespace UrlShortener.Application.Interfaces;

public interface IUrlService
{
    Task<CreateShortUrlResponse> CreateAsync(
        CreateShortUrlRequest request,
        CancellationToken cancellationToken);
}
