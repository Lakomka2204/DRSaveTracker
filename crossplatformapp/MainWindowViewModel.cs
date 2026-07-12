using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DRSTCore;
namespace crossplatformapp;

public partial class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        Sfw = new()
        {
            BackupDirectory = backupFolder,
            CacheDirectory = cacheFolder,
            WatchDirectory = saveFolder,
            IsEnabled = true,
        };
    }
    public async Task Initialize()
    {
        await Sfw.Init();
    }
    [ObservableProperty]
    private SaveFileWatcher sfw;
    [ObservableProperty]
    private SaveFileInfo? selectedSave;
    [ObservableProperty]
    private SaveFileInfo? selectedBackup;
    private static readonly string saveFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "DELTARUNE");
    private static readonly string backupFolder = Path.Join(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData),
        Program.AppName,
        "Backups");
    private static readonly string cacheFolder = Path.Join(
        Environment.GetFolderPath(
            Environment.SpecialFolder.LocalApplicationData),
        Program.AppName,
        "Cache");
    [ObservableProperty]
    private static Avalonia.Media.FontFamily fontFamily = new("fonts:DRFonts#8-bit Operator+");
    [RelayCommand]
    private static void QuitApp()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown(0);
        }
    }
    [RelayCommand]
    private void ToggleWatching()
    {
        Sfw.IsEnabled = !Sfw.IsEnabled;
        System.Console.WriteLine("Toggling thang {0}",Sfw.IsEnabled);
    }
}