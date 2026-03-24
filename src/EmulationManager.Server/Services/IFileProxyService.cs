using EmulationManager.Shared.Enums;

namespace EmulationManager.Server.Services;

public record FileDownloadInfo(
    string PhysicalPath,
    string FileName,
    long FileSize,
    string ContentType
);

public interface IFileProxyService
{
    Task<FileDownloadInfo?> ResolveFileAsync(DownloadableType type, int id);
}
