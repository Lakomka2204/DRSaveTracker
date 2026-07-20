using CommunityToolkit.Mvvm.ComponentModel;

namespace DRSTCore;

public partial class SaveFileBackup : ObservableObject
{
    [ObservableProperty]
    private string fileName = string.Empty;
    [ObservableProperty]
    private DateTime lastWrite = DateTime.UnixEpoch;
    [ObservableProperty]
    private string name = "EMPTY";
    [ObservableProperty]
    private TimeSpan playTime = TimeSpan.Zero;
    [ObservableProperty]
    private int room = 0;
    [ObservableProperty]
    private string roomName = "N/A";
}