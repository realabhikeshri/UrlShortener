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

    [HttpGet("/{shortCode:regex(^[a-zA-Z0-9]{{6,10}}$)}")]
    public async Task<IActionResult> RedirectToLongUrl(string shortCode)
    {
        var longUrl = await _service.ResolveAsync(
    shortCode,
    HttpContext.RequestAborted
);


        if (longUrl == null)
            return NotFound();

        // HTTP 302 redirect
        return Redirect(longUrl);
    }
}
