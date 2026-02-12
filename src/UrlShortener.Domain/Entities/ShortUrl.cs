namespace UrlShortener.Domain.Entities;

public class ShortUrl
{
    public Guid Id { get; set; }
    public string ShortCode { get; set; } = null!;
    public string LongUrl { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; }
}
