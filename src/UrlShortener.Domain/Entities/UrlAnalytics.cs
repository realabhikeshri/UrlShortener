public class UrlAnalytics
{
    public Guid Id { get; private set; }
    public Guid ShortUrlId { get; private set; }
    public int ClickCount { get; private set; }

    // EF Core requires a parameterless constructor
    private UrlAnalytics() { }

    public UrlAnalytics(Guid shortUrlId)
    {
        Id = Guid.NewGuid();
        ShortUrlId = shortUrlId;
        ClickCount = 1;
    }

    public void Increment()
    {
        ClickCount++;
    }
}
