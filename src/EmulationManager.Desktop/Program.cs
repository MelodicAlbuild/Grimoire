using Avalonia;
using EmulationManager.Desktop.Services;

namespace EmulationManager.Desktop;

public static class Program
{
    /// <summary>
    /// Stores the emumgr:// URI if the app was launched via protocol activation.
    /// Checked by App.OnFrameworkInitializationCompleted to trigger the launch flow.
    /// </summary>
    public static string? ProtocolActivationUri { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
        // Check for protocol activation: emumgr://launch/{gameId}
        if (args.Length > 0 && args[0].StartsWith("emumgr://", StringComparison.OrdinalIgnoreCase))
        {
            ProtocolActivationUri = args[0];
        }

        // Register protocol handler on first run
        var protocolHandler = new ProtocolHandler();
        if (!protocolHandler.IsProtocolRegistered())
        {
            var exePath = Environment.ProcessPath;
            if (exePath is not null)
            {
                protocolHandler.RegisterProtocol(exePath);
            }
        }

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
