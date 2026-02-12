namespace UrlShortener.Application.Interfaces;

public interface IUrlRedirectService
{
    Task<string?> ResolveAsync(string shortCode, CancellationToken cancellationToken);

}
