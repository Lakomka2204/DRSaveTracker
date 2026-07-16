using System.Linq;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
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
    private static void QuitApp()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown(0);
        }
    }
}