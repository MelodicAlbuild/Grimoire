using Avalonia.Controls;
using EmulationManager.Desktop.ViewModels;

namespace EmulationManager.Desktop.Views;

public partial class GameLibraryView : UserControl
{
    public GameLibraryView()
    {
        InitializeComponent();
    }

    protected override async void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        if (DataContext is GameLibraryViewModel vm)
        {
            await vm.LoadGamesCommand.ExecuteAsync(null);
        }
    }
}
