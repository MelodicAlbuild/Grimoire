namespace Grimoire.Server.Entities;

/// <summary>
/// Tracks a platform-specific emulator binary file (e.g., Ryubing for win-x64).
/// Directory convention: {EmulatorsBasePath}/{emulator-name}/{rid}/{filename}
/// Example: /mnt/storage/emulation/emulators/ryubing/win-x64/Ryubing-1.2.82.zip
/// </summary>
public class EmulatorBinaryEntity
{
    public int Id { get; set; }
    public int EmulatorId { get; set; }

    /// <summary>Runtime identifier: win-x64, linux-x64, osx-arm64</summary>
    public required string RuntimeId { get; set; }

    /// <summary>Path relative to EmulatorsBasePath</summary>
    public required string FilePath { get; set; }

    public long FileSize { get; set; }
    public string? FileHash { get; set; }

    public EmulatorEntity Emulator { get; set; } = null!;
}
