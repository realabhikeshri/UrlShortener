using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/urls/{shortCode}/analytics")]
public class AnalyticsController : ControllerBase
{
    private readonly IUrlAnalyticsService _service;

    public AnalyticsController(IUrlAnalyticsService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        string shortCode,
        CancellationToken cancellationToken)
    {
        var clicks = await _service.GetAsync(shortCode, cancellationToken);

        return Ok(new
        {
            shortCode,
            clicks
        });
    }
}
