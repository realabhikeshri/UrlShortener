namespace UrlShortener.Application.Dtos;

public record DailyClickDto(
    DateOnly Date,
    long Count
);
