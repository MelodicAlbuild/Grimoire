using Grimoire.Server.Data;
using Grimoire.Server.Entities;
using Grimoire.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Grimoire.Server.Services;

public interface ILibraryScanService
{
    Task<ScanResult> ScanAsync(CancellationToken ct = default);
}

public record ScanResult(int ImportedCount, int ScannedCount, string? Error = null);

/// <summary>
/// Background service that scans configured storage directories
/// and auto-imports games into the database.
/// </summary>
public class LibraryScanService : BackgroundService, ILibraryScanService
{
    private readonly IServiceProvider _services;
    private readonly ILogger<LibraryScanService> _logger;
    private readonly StorageOptions _storage;

    private static readonly Dictionary<string, PlatformType> ExtensionToPlatform = new(StringComparer.OrdinalIgnoreCase)
    {
        [".nsp"] = PlatformType.NintendoSwitch,
        [".xci"] = PlatformType.NintendoSwitch,
        [".nca"] = PlatformType.NintendoSwitch,
        [".nds"] = PlatformType.NintendoDS,
        [".dsi"] = PlatformType.NintendoDS,
        [".3ds"] = PlatformType.Nintendo3DS,
        [".cci"] = PlatformType.Nintendo3DS,
        [".cxi"] = PlatformType.Nintendo3DS,
        [".cia"] = PlatformType.Nintendo3DS,
        [".gb"] = PlatformType.GameBoy,
        [".gbc"] = PlatformType.GameBoy,
        [".gba"] = PlatformType.GameBoy,
    };

    public LibraryScanService(IServiceProvider services, ILogger<LibraryScanService> logger,
        IOptions<StorageOptions> storage)
    {
        _services = services;
        _logger = logger;
        _storage = storage.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        await ScanAsync(stoppingToken);
    }

    public async Task<ScanResult> ScanAsync(CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_storage.GamesBasePath))
        {
            _logger.LogInformation("Games base path not configured, skipping library scan");
            return new ScanResult(0, 0, "Games base path not configured");
        }

        if (!Directory.Exists(_storage.GamesBasePath))
        {
            _logger.LogWarning("Games base path does not exist: {Path}", _storage.GamesBasePath);
            return new ScanResult(0, 0, $"Path does not exist: {_storage.GamesBasePath}");
        }

        _logger.LogInformation("Starting library scan of {Path}", _storage.GamesBasePath);

        using var scope = _services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<GrimoireDbContext>();

        var existingPaths = await db.Games.Select(g => g.FilePath).ToHashSetAsync(ct);
        var newGames = new List<GameEntity>();
        var scannedCount = 0;

        foreach (var file in Directory.EnumerateFiles(_storage.GamesBasePath, "*.*", SearchOption.AllDirectories))
        {
            var ext = Path.GetExtension(file);
            if (!ExtensionToPlatform.TryGetValue(ext, out var platform))
                continue;

            scannedCount++;
            var relativePath = Path.GetRelativePath(_storage.GamesBasePath, file);
            if (existingPaths.Contains(relativePath))
                continue;

            var fileInfo = new FileInfo(file);
            var title = Path.GetFileNameWithoutExtension(file)
                .Replace('_', ' ')
                .Replace('-', ' ');

            newGames.Add(new GameEntity
            {
                Title = title,
                Platform = platform,
                FilePath = relativePath,
                FileSize = fileInfo.Length
            });
        }

        if (newGames.Count > 0)
        {
            db.Games.AddRange(newGames);
            await db.SaveChangesAsync(ct);
            _logger.LogInformation("Library scan complete: imported {Count} new games out of {Scanned} scanned",
                newGames.Count, scannedCount);
        }
        else
        {
            _logger.LogInformation("Library scan complete: no new games found ({Scanned} scanned)", scannedCount);
        }

        return new ScanResult(newGames.Count, scannedCount);
    }
}
