using EmulationManager.Shared.Enums;

namespace EmulationManager.Server.Entities;

public class FirmwareEntity
{
    public int Id { get; set; }
    public PlatformType Platform { get; set; }
    public required string Version { get; set; }
    public required string FilePath { get; set; }
}
