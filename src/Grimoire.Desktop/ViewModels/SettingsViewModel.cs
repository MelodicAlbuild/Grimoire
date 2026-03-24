using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grimoire.Desktop.Services;

namespace Grimoire.Desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly ISettingsService _settings;

    [ObservableProperty]
    private string _serverUrl = "https://emu.melodicalbuild.com";

    [ObservableProperty]
    private string _installDirectory = "";

    [ObservableProperty]
    private string? _statusMessage;

    public SettingsViewModel(ISettingsService settings)
    {
        _settings = settings;
    }

    [RelayCommand]
    private async Task LoadSettings()
    {
        ServerUrl = await _settings.GetServerUrlAsync();
        InstallDirectory = await _settings.GetInstallDirectoryAsync();
    }

    [RelayCommand]
    private async Task SaveSettings()
    {
        await _settings.SetServerUrlAsync(ServerUrl);
        await _settings.SetAsync(SettingsService.InstallDirectoryKey, InstallDirectory);
        StatusMessage = "Settings saved.";
    }

    [RelayCommand]
    private async Task BrowseInstallDirectory()
    {
        var topLevel = TopLevel.GetTopLevel(App.Services.GetService(typeof(Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime)) is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null);

        if (topLevel is null) return;

        var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            Title = "Select Install Directory",
            AllowMultiple = false
        });

        if (folders.Count > 0)
        {
            InstallDirectory = folders[0].Path.LocalPath;
        }
    }
}
