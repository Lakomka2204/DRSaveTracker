using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media;
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
        IsSysFont = Settings.Current.IsSysFont;
    }
    [ObservableProperty]
    private bool isSysFont;
    partial void OnIsSysFontChanged(bool value)
    {
        if (Application.Current is null) return;
        FontFamily ff;
        if (value)
            ff = FontFamily.Default;
        else
            ff = new FontFamily("fonts:DRFonts#8-bit Operator+, fonts:DRFonts#JF Dot Shinonome Gothic 14");
        Application.Current.Resources["DRFontFamily"] = ff;
        Settings.Current.IsSysFont = value;
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