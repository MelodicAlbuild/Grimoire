using Avalonia.Controls;
using EmulationManager.Desktop.ViewModels;

namespace EmulationManager.Desktop.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    protected override async void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is SettingsViewModel vm)
        {
            await vm.LoadSettingsCommand.ExecuteAsync(null);
        }
    }
}
