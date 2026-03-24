using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.DTOs;

public record EmulatorDto(
    int Id,
    string Name,
    PlatformType Platform,
    string Version,
    string ExecutableName
);
