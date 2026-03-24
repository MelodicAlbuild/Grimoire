using EmulationManager.Shared.Enums;

namespace EmulationManager.Shared.DTOs;

public record DownloadRequestDto(
    DownloadableType Type,
    int Id
);
