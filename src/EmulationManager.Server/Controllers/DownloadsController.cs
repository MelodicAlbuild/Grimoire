using EmulationManager.Server.Services;
using EmulationManager.Shared.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EmulationManager.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DownloadsController : ControllerBase
{
    private readonly IFileProxyService _fileProxy;

    public DownloadsController(IFileProxyService fileProxy)
    {
        _fileProxy = fileProxy;
    }

    [HttpGet("{type}/{id:int}")]
    public async Task<IActionResult> Download(DownloadableType type, int id)
    {
        var fileInfo = await _fileProxy.ResolveFileAsync(type, id);
        if (fileInfo is null)
            return NotFound();

        if (!System.IO.File.Exists(fileInfo.PhysicalPath))
            return NotFound(new { error = "File not found on storage", path = fileInfo.FileName });

        return PhysicalFile(
            fileInfo.PhysicalPath,
            fileInfo.ContentType,
            fileInfo.FileName,
            enableRangeProcessing: true
        );
    }

    [HttpHead("{type}/{id:int}")]
    public async Task<IActionResult> GetFileInfo(DownloadableType type, int id)
    {
        var fileInfo = await _fileProxy.ResolveFileAsync(type, id);
        if (fileInfo is null)
            return NotFound();

        Response.Headers.ContentLength = fileInfo.FileSize;
        Response.Headers["Accept-Ranges"] = "bytes";
        Response.ContentType = fileInfo.ContentType;
        Response.Headers.ContentDisposition = $"attachment; filename=\"{fileInfo.FileName}\"";
        return Ok();
    }
}
