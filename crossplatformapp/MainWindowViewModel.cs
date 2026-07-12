using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
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
        rmi = new(cacheFolder);
        saves = [];
        InitializeSaves();
    }
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
    private readonly RoomMapper rmi;
    private async void InitializeSaves()
    {
        await rmi.LoadRoomInfo();
        var saves = Directory.GetFiles(saveFolder, "filech*")
                .Select(s => new SaveFileInfo(s, rmi))
                .Where(s => s.FileExists);
                Console.WriteLine("get saves {0}",saves.Count());
        if (!Directory.Exists(backupFolder))
        {
            foreach (var save in saves)
            {
                save.MakeBackup(backupFolder);
            }
        }
        if (!Directory.Exists(backupFolder))
            Directory.CreateDirectory(backupFolder);
        var backups = Directory
            .GetDirectories(
                backupFolder,
                "filech*",
                SearchOption.TopDirectoryOnly)
            .Select(s => new SaveFileInfo(s, rmi));
        var allSaves = saves
            .Concat(backups)
            .GroupBy(g => g.OriginalFileName)
            .Select(g => g.First())
            .Select(s => new SVIShort(s));
            System.Console.WriteLine("get all saves {0}",allSaves.Count());
        Saves = new ObservableCollection<SVIShort>(allSaves);
    }
    [ObservableProperty]
    private ObservableCollection<SVIShort> saves;
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
}