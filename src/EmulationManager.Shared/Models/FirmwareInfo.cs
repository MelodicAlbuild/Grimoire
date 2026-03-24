using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.Models;

public record FirmwareInfo(
    int Id,
    PlatformType Platform,
    string Version,
    string? DownloadUrl
);
