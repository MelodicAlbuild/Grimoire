using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.Models;

public record BiosInfo(
    int Id,
    PlatformType Platform,
    string FileName,
    string? Description
);
