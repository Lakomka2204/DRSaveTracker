using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Themes.Fluent;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using crossplatformapp.Utils;

namespace crossplatformapp;

public partial class MenuViewModel : ObservableObject
{
    public MenuViewModel()
    {
        IsCompact = Settings.Current.IsCompact;
    }
    [ObservableProperty]
    private bool isCompact;
    partial void OnIsCompactChanged(bool value)
    {
        DensityStyle d = value ? DensityStyle.Compact : DensityStyle.Normal;
        SetDensity(d);
        Settings.Current.IsCompact = value;
    }

    private static void SetDensity(DensityStyle density)
    {
        var theme = Application.Current?.Styles
            .OfType<FluentTheme>()
            .FirstOrDefault();

        if (theme != null)
            theme.DensityStyle = density;
    }
    [RelayCommand]
    private static void HideWindow()
    {
        Program.GetLife()?.MainWindow?.Hide();
    }
    [RelayCommand]
    private static async Task OpenSettings()
    {
        SettingsWindow sw = new();
        await sw.ShowDialog(Program.GetLife()?.MainWindow!);
    }
    [RelayCommand]
    private static void QuitApp()
    {
        Program.GetLife()?.Shutdown(0);
    }
}