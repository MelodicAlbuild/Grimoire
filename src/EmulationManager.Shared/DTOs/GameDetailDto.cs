using EmulationManager.Shared.Enums;
using EmulationManager.Shared.Models;

namespace EmulationManager.Shared.DTOs;

public record GameDetailDto(
    int Id,
    string Title,
    PlatformType Platform,
    string? Description,
    string? CoverImageUrl,
    long FileSize,
    string? FileHash,
    IReadOnlyList<DlcInfo> Dlcs,
    IReadOnlyList<UpdateInfo> Updates
);
