using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.Models;

public record EmulatorInfo(
    int Id,
    string Name,
    PlatformType Platform,
    string Version,
    string? DownloadUrl,
    string ExecutableName
);
