namespace EmulationManager.Shared.Models;

public record LibraryItem(
    GameInfo Game,
    IReadOnlyList<DlcInfo> Dlcs,
    IReadOnlyList<UpdateInfo> Updates
);
