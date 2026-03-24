using Grimoire.Server.Data;
using Grimoire.Server.Services;
using Grimoire.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grimoire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmulatorsController : ControllerBase
{
    private readonly IGameService _gameService;
    private readonly GrimoireDbContext _db;
    private readonly StorageOptions _storage;
    private readonly ILogger<EmulatorsController> _logger;

    public EmulatorsController(IGameService gameService, GrimoireDbContext db,
        IOptions<StorageOptions> storage, ILogger<EmulatorsController> logger)
    {
        _gameService = gameService;
        _db = db;
        _storage = storage.Value;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmulators()
    {
        var emulators = await _gameService.GetEmulatorsAsync();
        return Ok(emulators);
    }

    [HttpGet("{platform}")]
    public async Task<IActionResult> GetEmulator(PlatformType platform)
    {
        var emulator = await _gameService.GetEmulatorByPlatformAsync(platform);
        if (emulator is null)
            return NotFound();
        return Ok(emulator);
    }

    [HttpGet("{platform}/download/{runtimeId}")]
    [EnableRateLimiting("downloads")]
    public async Task<IActionResult> DownloadEmulator(PlatformType platform, string runtimeId)
    {
        var binary = await _db.EmulatorBinaries
            .Include(b => b.Emulator)
            .FirstOrDefaultAsync(b => b.Emulator.Platform == platform && b.RuntimeId == runtimeId);

        if (binary is null)
            return NotFound(new { error = $"No binary for {platform}/{runtimeId}" });

        var physicalPath = Path.IsPathRooted(binary.FilePath)
            ? binary.FilePath
            : Path.Combine(_storage.EmulatorsBasePath, binary.FilePath);

        if (!System.IO.File.Exists(physicalPath))
        {
            _logger.LogWarning("Emulator binary not found: {Path}", physicalPath);
            return NotFound(new { error = "Binary file not found on storage" });
        }

        _logger.LogInformation("Serving emulator: {Name} {RuntimeId} from {Path}",
            binary.Emulator.Name, runtimeId, physicalPath);

        return PhysicalFile(physicalPath, "application/octet-stream",
            Path.GetFileName(binary.FilePath), enableRangeProcessing: true);
    }

    [HttpHead("{platform}/download/{runtimeId}")]
    public async Task<IActionResult> GetEmulatorFileInfo(PlatformType platform, string runtimeId)
    {
        var binary = await _db.EmulatorBinaries
            .Include(b => b.Emulator)
            .FirstOrDefaultAsync(b => b.Emulator.Platform == platform && b.RuntimeId == runtimeId);

        if (binary is null)
            return NotFound();

        Response.Headers.ContentLength = binary.FileSize;
        Response.Headers["Accept-Ranges"] = "bytes";
        Response.ContentType = "application/octet-stream";
        return Ok();
    }
}
