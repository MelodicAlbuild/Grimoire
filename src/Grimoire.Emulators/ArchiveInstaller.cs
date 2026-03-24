using System.IO.Compression;

namespace Grimoire.Emulators;

/// <summary>
/// Shared helper for extracting emulator archives (ZIP) to an install directory.
/// </summary>
public static class ArchiveInstaller
{
    public static async Task ExtractZipAsync(string archivePath, string installDirectory,
        IProgress<double> progress, CancellationToken ct = default)
    {
        await Task.Run(() =>
        {
            using var archive = ZipFile.OpenRead(archivePath);
            var totalEntries = archive.Entries.Count;
            var processed = 0;

            foreach (var entry in archive.Entries)
            {
                ct.ThrowIfCancellationRequested();

                var destPath = Path.Combine(installDirectory, entry.FullName);

                // Skip directory entries
                if (string.IsNullOrEmpty(entry.Name))
                {
                    Directory.CreateDirectory(destPath);
                    continue;
                }

                // Flatten single top-level directory if present
                // e.g., "Ryubing-1.2.82/Ryujinx.exe" -> "Ryujinx.exe"
                var parts = entry.FullName.Replace('\\', '/').Split('/');
                if (parts.Length > 1 && totalEntries > 1)
                {
                    // Check if all entries share a common root dir
                    var firstDir = archive.Entries
                        .Select(e => e.FullName.Replace('\\', '/').Split('/')[0])
                        .Distinct()
                        .ToList();

                    if (firstDir.Count == 1)
                    {
                        // Strip the common root
                        var stripped = string.Join('/', parts.Skip(1));
                        if (string.IsNullOrEmpty(stripped)) continue;
                        destPath = Path.Combine(installDirectory, stripped);
                    }
                }

                Directory.CreateDirectory(Path.GetDirectoryName(destPath)!);
                entry.ExtractToFile(destPath, overwrite: true);

                processed++;
                progress.Report((double)processed / totalEntries * 100.0);
            }
        }, ct);
    }
}
