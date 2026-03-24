using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.DTOs;

public record GameListDto(
    int Id,
    string Title,
    PlatformType Platform,
    string? CoverImageUrl,
    bool HasDlc,
    bool HasUpdates
);
