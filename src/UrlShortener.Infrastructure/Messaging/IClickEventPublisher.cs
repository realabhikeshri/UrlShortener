namespace UrlShortener.Infrastructure.Messaging;

public interface IClickEventPublisher
{
    Task PublishAsync(ClickEvent clickEvent);
}
