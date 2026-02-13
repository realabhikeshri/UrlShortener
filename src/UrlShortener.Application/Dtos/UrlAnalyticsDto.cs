namespace UrlShortener.Application.Dtos;

public record UrlAnalyticsDto(
    string ShortCode,
    long TotalClicks,
    IReadOnlyList<DailyClickDto> DailyClicks
);
