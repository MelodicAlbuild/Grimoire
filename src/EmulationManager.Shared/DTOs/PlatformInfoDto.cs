using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.DTOs;

public record PlatformInfoDto(
    PlatformType Type,
    string DisplayName,
    int GameCount
);
