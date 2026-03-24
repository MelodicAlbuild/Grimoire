using EmulationManager.Shared.DTOs;
using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.Interfaces;

/// <summary>
/// Defines the API contract for communicating with the EmulationManager server.
/// Used by both the desktop client and Blazor server-side services.
/// </summary>
public interface IEmulationManagerApi
{
    // -- Games --

    Task<IReadOnlyList<GameListDto>> GetGamesAsync(PlatformType? platform = null,
        string? search = null, CancellationToken ct = default);

    Task<GameDetailDto?> GetGameDetailAsync(int gameId, CancellationToken ct = default);

    // -- Emulators --

    Task<IReadOnlyList<EmulatorDto>> GetEmulatorsAsync(CancellationToken ct = default);

    Task<EmulatorDto?> GetEmulatorAsync(PlatformType platform, CancellationToken ct = default);

    // -- Platforms --

    Task<IReadOnlyList<PlatformInfoDto>> GetPlatformsAsync(CancellationToken ct = default);

    // -- Downloads --

    Task<Stream> GetDownloadStreamAsync(DownloadableType type, int id,
        long? rangeStart = null, CancellationToken ct = default);

    Task<long> GetDownloadSizeAsync(DownloadableType type, int id,
        CancellationToken ct = default);

    // -- Client Version --

    Task<ClientVersionDto?> GetLatestClientVersionAsync(CancellationToken ct = default);
}
