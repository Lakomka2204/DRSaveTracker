
using DRSTCore; 
string appName = "DRSaveTracker";

string saveFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            "DELTARUNE");
        string backupFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            appName,
            "Backups");
        string cacheFolder = Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            appName,
            "Cache");


Console.WriteLine("APP START");
Console.WriteLine("SAVE DIR {0}", saveFolder);
Console.WriteLine("BACKUP DIR {0}",backupFolder);
Console.WriteLine("CACHE DIR {0}",cacheFolder);

SaveFileWatcher sfw = new()
{
    BackupDirectory = backupFolder,
    CacheDirectory = cacheFolder,
    WatchDirectory = saveFolder
};
Console.WriteLine("SFW DECLARED");
sfw.IsEnabled = true;
Console.WriteLine(string.Join("\n",sfw.Saves));
Console.WriteLine("SFW ENABLED");
Console.ReadLine();