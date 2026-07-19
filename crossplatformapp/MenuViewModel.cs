using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Themes.Fluent;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace crossplatformapp;

public partial class MenuViewModel : ObservableObject
{
    [ObservableProperty]
    private bool normalDensity = true;

    [ObservableProperty]
    private bool compactDensity;

    partial void OnNormalDensityChanged(bool value)
    {
        if (value)
            SetDensity(DensityStyle.Normal);
    }

    partial void OnCompactDensityChanged(bool value)
    {
        if (value)
            SetDensity(DensityStyle.Compact);
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
        SettingsWindow sw =new();
        await sw.ShowDialog(Program.GetLife()?.MainWindow!);
    }
    [RelayCommand]
    internal static void QuitApp()
    {
        Program.GetLife()?.Shutdown(0);
    }
}