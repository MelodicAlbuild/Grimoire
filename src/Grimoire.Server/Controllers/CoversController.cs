using Grimoire.Server.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grimoire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoversController : ControllerBase
{
    private readonly GrimoireDbContext _db;
    private readonly StorageOptions _storage;

    public CoversController(GrimoireDbContext db, IOptions<StorageOptions> storage)
    {
        _db = db;
        _storage = storage.Value;
    }

    [HttpGet("{gameId:int}")]
    [ResponseCache(Duration = 86400)] // Cache 24h — cover art rarely changes
    public async Task<IActionResult> GetCover(int gameId)
    {
        var game = await _db.Games.FindAsync(gameId);
        if (game?.CoverImagePath is null)
            return NotFound();

        var physicalPath = Path.IsPathRooted(game.CoverImagePath)
            ? game.CoverImagePath
            : Path.Combine(_storage.CoverImagesBasePath, game.CoverImagePath);

        if (!System.IO.File.Exists(physicalPath))
            return NotFound();

        var contentType = Path.GetExtension(physicalPath).ToLowerInvariant() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

        return PhysicalFile(physicalPath, contentType);
    }
}
