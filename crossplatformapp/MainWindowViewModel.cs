using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using crossplatformapp.Utils;
using DRSTCore;
namespace crossplatformapp;

public partial class MainWindowViewModel : ObservableObject
{
    private readonly Window mainWindow;
    public MainWindowViewModel(Window window)
    {
        mainWindow = window;
        Sfw = new()
        {
            BackupDirectory = PlatformPaths.BackupDirectory,
            CacheDirectory = PlatformPaths.CacheDirectory,
            WatchDirectory = PlatformPaths.SaveDirectory,
            IsEnabled = true,
        };
        Menu = new();
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
        Console.WriteLine("Toggling thang {0}", Sfw.IsEnabled);
    }
    
    [RelayCommand]
    private async Task RestoreSelectedBackup()
    {
        if (SelectedBackup == null) return;
        if (SelectedSave == null) return;
        if (SelectedBackup.LastWrite == SelectedSave.LastWrite)
        {
            await MessageBoxes.GetOwnMBox(
                "Can't restore this backup",
                "This is the latest backup, no need to restore", 
                MsBox.Avalonia.Enums.Icon.Warning
            )
            .ShowAsPopupAsync(mainWindow);
            return;
        }
        TimeConverter tc = new();
        var mbox = await MessageBoxes.GetOwnMboxYesNo(
            "Restore backup?",
            string.Format(
                "Do you want to restore this backup to Ch. {0} slot {1}?\n"
                +"{2} {3} -> {4} {5}",
                SelectedBackup.Chapter,
                SelectedBackup.Slot,
                SelectedBackup.Name,
                tc.Convert(SelectedBackup.PlayTime
                ,typeof(string),null,CultureInfo.CurrentCulture),
                SelectedSave.Name,
                tc.Convert(SelectedSave.PlayTime
                ,typeof(string),null,CultureInfo.CurrentCulture)
                ),
                MsBox.Avalonia.Enums.Icon.Question
        ).ShowAsPopupAsync(mainWindow);
        if (mbox == "Yes")
        {
            Sfw.IsEnabled = false;
            var original = SelectedBackup.RestoreToOriginal(PlatformPaths.SaveDirectory);
            Sfw.UpdateFileInfo(original);
            Sfw.IsEnabled = true;
        }
    }
    [RelayCommand]
    private async Task DeleteBackup()
    {
        if (SelectedBackup == null) return;
        if (SelectedSave == null) return;
        var mbox = await MessageBoxes
            .GetOwnMboxYesNo(
                "Delete backup?",
                string.Format(
                    "Do you want to delete backup from {0:G}?\nThis action is irreversible.",
                    SelectedBackup.LastWrite
                ),
                MsBox.Avalonia.Enums.Icon.Question
            )
            .ShowAsPopupAsync(mainWindow);
        if (mbox == "Yes")
        {
            File.Delete(SelectedBackup.FileName);
            SelectedSave.GetBackups(PlatformPaths.BackupDirectory);
            System.Console.WriteLine("hafta delete it..");
        }
    }
    public MenuViewModel Menu { get; }
}