namespace EmulationManager.Shared.DTOs;

public record ClientVersionDto(
    string Version,
    Dictionary<string, string> DownloadUrls
);
