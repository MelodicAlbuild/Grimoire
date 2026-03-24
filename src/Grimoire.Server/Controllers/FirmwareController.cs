using Grimoire.Server.Data;
using Grimoire.Shared.DTOs;
using Grimoire.Shared.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Grimoire.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FirmwareController : ControllerBase
{
    private readonly GrimoireDbContext _db;

    public FirmwareController(GrimoireDbContext db) => _db = db;

    [HttpGet("{platform}")]
    public async Task<IActionResult> GetFirmware(PlatformType platform)
    {
        var firmware = await _db.Firmwares
            .Where(f => f.Platform == platform)
            .Select(f => new FirmwareDto(f.Id, f.Platform, f.Version))
            .ToListAsync();
        return Ok(firmware);
    }
}

[ApiController]
[Route("api/[controller]")]
public class BiosController : ControllerBase
{
    private readonly GrimoireDbContext _db;

    public BiosController(GrimoireDbContext db) => _db = db;

    [HttpGet("{platform}")]
    public async Task<IActionResult> GetBiosFiles(PlatformType platform)
    {
        var bios = await _db.BiosFiles
            .Where(b => b.Platform == platform)
            .Select(b => new BiosFileDto(b.Id, b.Platform, b.FileName, b.Description))
            .ToListAsync();
        return Ok(bios);
    }
}
