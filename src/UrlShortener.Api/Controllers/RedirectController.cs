using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly IUrlRedirectService _service;

    public RedirectController(IUrlRedirectService service)
    {
        _service = service;
    }

    [HttpGet("/{shortCode}")]
    public async Task<IActionResult> RedirectToLongUrl(string shortCode)
    {
        // Validate in code instead of route
        if (shortCode.Length < 6 || shortCode.Length > 10)
            return NotFound();

        var longUrl = await _service.ResolveAsync(
            shortCode,
            HttpContext.RequestAborted
        );

        if (longUrl is null)
            return NotFound();

        return Redirect(longUrl);
    }
}
