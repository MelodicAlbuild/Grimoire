using EmulationManager.Server.Data;
using EmulationManager.Shared.DTOs;
using EmulationManager.Shared.Enums;
using EmulationManager.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EmulationManager.Server.Services;

public class GameService : IGameService
{
    private readonly EmulationManagerDbContext _db;

    public GameService(EmulationManagerDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<GameListDto>> GetGamesAsync(PlatformType? platform = null, string? search = null)
    {
        var query = _db.Games.AsQueryable();

        if (platform.HasValue)
            query = query.Where(g => g.Platform == platform.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(g => g.Title.Contains(search));

        return await query
            .OrderBy(g => g.Title)
            .Select(g => new GameListDto(
                g.Id,
                g.Title,
                g.Platform,
                g.CoverImagePath,
                g.Dlcs.Any(),
                g.Updates.Any()
            ))
            .ToListAsync();
    }

    public async Task<GameDetailDto?> GetGameDetailAsync(int gameId)
    {
        var game = await _db.Games
            .Include(g => g.Dlcs)
            .Include(g => g.Updates)
            .FirstOrDefaultAsync(g => g.Id == gameId);

        if (game is null)
            return null;

        return new GameDetailDto(
            game.Id,
            game.Title,
            game.Platform,
            game.Description,
            game.CoverImagePath,
            game.FileSize,
            game.FileHash,
            game.Dlcs.Select(d => new DlcInfo(d.Id, d.GameId, d.Title, d.Version, d.FileSize)).ToList(),
            game.Updates.Select(u => new UpdateInfo(u.Id, u.GameId, u.Title, u.Version, u.FileSize)).ToList()
        );
    }

    public async Task<IReadOnlyList<PlatformInfoDto>> GetPlatformStatsAsync()
    {
        return await _db.Games
            .GroupBy(g => g.Platform)
            .Select(g => new PlatformInfoDto(
                g.Key,
                g.Key.ToString(),
                g.Count()
            ))
            .ToListAsync();
    }

    public async Task<IReadOnlyList<EmulatorDto>> GetEmulatorsAsync()
    {
        return await _db.Emulators
            .Select(e => new EmulatorDto(e.Id, e.Name, e.Platform, e.Version, e.ExecutableName))
            .ToListAsync();
    }

    public async Task<EmulatorDto?> GetEmulatorByPlatformAsync(PlatformType platform)
    {
        var emu = await _db.Emulators.FirstOrDefaultAsync(e => e.Platform == platform);
        if (emu is null) return null;
        return new EmulatorDto(emu.Id, emu.Name, emu.Platform, emu.Version, emu.ExecutableName);
    }

    public async Task<int> GetGameCountAsync()
    {
        return await _db.Games.CountAsync();
    }
}
