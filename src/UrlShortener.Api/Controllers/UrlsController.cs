using Microsoft.AspNetCore.Mvc;
using UrlShortener.Application.Dtos;
using UrlShortener.Application.Interfaces;

namespace UrlShortener.Api.Controllers;

[ApiController]
[Route("api/urls")]
public class UrlsController : ControllerBase
{
    private readonly IUrlService _service;

    public UrlsController(IUrlService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<CreateShortUrlResponse>> Create(
        [FromBody] CreateShortUrlRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(request, cancellationToken);
        return Ok(result);
    }
}
