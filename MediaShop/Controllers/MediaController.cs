using MediaShop.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaShop.Controllers;

[Route("api/media")]
[ApiController]
public class MediaController(IMediaService mediaService) : ControllerBase
{
    private readonly IMediaService _mediaService = mediaService;

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        return Ok(await _mediaService.UploadAsync(file));
    }
}
