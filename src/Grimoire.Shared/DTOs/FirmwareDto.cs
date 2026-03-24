using Grimoire.Shared.Enums;

namespace Grimoire.Shared.DTOs;

public record FirmwareDto(
    int Id,
    PlatformType Platform,
    string Version
);

public record BiosFileDto(
    int Id,
    PlatformType Platform,
    string FileName,
    string? Description
);
