using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DRSTCore;

public partial class SaveFileWatcher : ObservableObject
{
    private static readonly string filePattern = "filech*";
    public SaveFileWatcher()
    {
        mainWatcher = new()
        {
            Filter = filePattern,
            Path = string.Empty,
        };
        mainWatcher.Changed += WatcherChanged;
        mainWatcher.Created += WatcherChanged;
        mainWatcher.Deleted += WatcherChanged;
        rmi = new();
        saves = [];
    }
    public async Task Init()
    {
        if (CacheDirectory != null)
            await rmi.LoadRoomInfo();
        if (WatchDirectory != string.Empty)
            RefreshInfo();
    }
    ~SaveFileWatcher()
    {
        if (mainWatcher != null)
        {
            mainWatcher.Changed -= WatcherChanged;
            mainWatcher.Created -= WatcherChanged;
            mainWatcher.Deleted -= WatcherChanged;
            mainWatcher.Dispose();
        }
        rmi?.UnloadRoomInfo();
        Saves?.Clear();
        
    }
    public void UpdateFileInfo(string filename,string? name=null)
    {
        Console.WriteLine("Called UFI w/ {0} {1}",filename,name);
        name ??= Path.GetFileName(filename);
        var changed = Saves.FirstOrDefault(f => f.OriginalFileName == name);
            if (changed == null)
                return;
            int ind = Saves.IndexOf(changed);
            // make backup
            changed = new(filename,rmi,BackupDirectory);
            changed.MakeBackup(BackupDirectory);
            //update ui
            Saves[ind] = changed;
            Console.WriteLine("Changed Save: {0}",changed);
    }
    private void WatcherChanged(object sender, FileSystemEventArgs args)
    {
        try
        {
            mainWatcher.EnableRaisingEvents = false;
            // do the thang
            Console.WriteLine("Something happened! {0} {1} {2}",args.ChangeType,args.FullPath,args.Name);
            if ((args.ChangeType & (WatcherChangeTypes.Created | WatcherChangeTypes.Deleted)) != 0)
                RefreshInfo();
            UpdateFileInfo(args.FullPath,args.Name);
        }
        finally
        {
            mainWatcher.EnableRaisingEvents = true;
        }
    }
    public void RefreshInfo()
    {
        // only run when WatchDirectory is set or updated
        if (!Directory.Exists(WatchDirectory))
        {
            Console.WriteLine("watch directory doesn't exists");
            return;
        }
        //! backupdirectory must be set before watchdirectory in order for this to work
        var saves = Directory
            .GetFiles(WatchDirectory,filePattern)
            .Select(f => new SaveFileInfo(f,rmi,BackupDirectory))
            .Where(s => s.FileExists);
        foreach(var save in saves)
        {
            if (save.Backups.Length == 0)
                save.MakeBackup(BackupDirectory);
        }
        var backups = Directory
            .GetDirectories(
                BackupDirectory,
                filePattern,
                SearchOption.TopDirectoryOnly)
            .Select(d => new SaveFileInfo(d,rmi,BackupDirectory));
            var allSaves = saves
                .Concat(backups)
                .GroupBy(s => s.OriginalFileName)
                .Select(sg => sg.First())
                .OrderBy(s => s.Chapter)
                .ThenBy(s => s.Slot);
            Saves = new ObservableCollection<SaveFileInfo>(allSaves);
    }
    private readonly RoomMapper rmi;
    private readonly FileSystemWatcher mainWatcher;
    [ObservableProperty]
    private ObservableCollection<SaveFileInfo> saves;
    public bool IsEnabled
    {
        get { return mainWatcher.EnableRaisingEvents; }
        set 
        { 
            mainWatcher.EnableRaisingEvents = value;
            OnPropertyChanged(nameof(IsEnabled));
        }
    }
    
    public string WatchDirectory 
    { 
        get { return mainWatcher.Path; } 
        set { mainWatcher.Path = value; }
    }
    public string BackupDirectory { get; set; } = string.Empty;
    public string? CacheDirectory 
    { 
        get { return rmi.CacheDirectory;}
        set{ rmi.CacheDirectory = value; }
    }
}