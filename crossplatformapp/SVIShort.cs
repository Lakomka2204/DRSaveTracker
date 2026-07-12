
using System;
using CommunityToolkit.Mvvm.ComponentModel;
using DRSTCore;

namespace crossplatformapp;
public partial class SVIShort : ObservableObject
{
    public SVIShort()
    {
        
    }
    public SVIShort(SaveFileInfo info)
    {
        slot = ((int)info.Slot)+1;
        name = info.Name;
        place = info.RoomName ?? string.Format("Room ID: {0}",info.Room);
        time = info.PlayTime;
        chapter = info.Chapter;
        LastAccess = info.LastWrite;
    }
    [ObservableProperty]
    private DateTime lastAccess = DateTime.UnixEpoch;
    [ObservableProperty]
    private int chapter = 0;
    [ObservableProperty]
    private int slot = 0;
    [ObservableProperty]
    private string name = "EMPTY";
    [ObservableProperty]
    private string place = "N/A";
    [ObservableProperty]
    private TimeSpan time = TimeSpan.Zero;
    public string DisplayTime => $"{Time.TotalHours:00}:{Time.Minutes:00}";
    partial void OnTimeChanged(TimeSpan value)
    {
        OnPropertyChanged(nameof(DisplayTime));
    }
}