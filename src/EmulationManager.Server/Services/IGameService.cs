using EmulationManager.Shared.DTOs;
using EmulationManager.Shared.Enums;

namespace EmulationManager.Server.Services;

public interface IGameService
{
    Task<IReadOnlyList<GameListDto>> GetGamesAsync(PlatformType? platform = null, string? search = null);
    Task<GameDetailDto?> GetGameDetailAsync(int gameId);
    Task<IReadOnlyList<PlatformInfoDto>> GetPlatformStatsAsync();
    Task<IReadOnlyList<EmulatorDto>> GetEmulatorsAsync();
    Task<EmulatorDto?> GetEmulatorByPlatformAsync(PlatformType platform);
    Task<int> GetGameCountAsync();
}
