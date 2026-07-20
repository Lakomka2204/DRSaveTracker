using System;
using System.IO;


namespace crossplatformapp.Utils;

public static class PlatformPaths
{
    public static string SaveDirectory
    {
        get
        {
            if (OperatingSystem.IsWindows())
                return Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            Program.GameName);
            else if (OperatingSystem.IsLinux())
                throw new NotImplementedException("AAAAAAAAAAAAAA test on linux you moron");
            else if (OperatingSystem.IsMacOS())
                throw new NotImplementedException("Buy me a new mac to test this out ");
            else throw new NotImplementedException("Platform not supported");
        }
    }
    public static string BaseDirectory
    {
        get
        {
            if (OperatingSystem.IsWindows())
                return Path.Join(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    Program.AppName);
            else if (OperatingSystem.IsLinux())
                throw new NotImplementedException("AAAAAAAAAAAAAA test on linux you moron");
            else if (OperatingSystem.IsMacOS())
                throw new NotImplementedException("Buy me a new mac to test this out ");
            else throw new NotImplementedException("Platform not supported");
        }
    }
    public static string BackupDirectory
    {
        get { return Path.Join(BaseDirectory, "Backups"); }
    }
    public static string CacheDirectory
    {
        get { return Path.Join(BaseDirectory, "Cache"); }
    }
    public static string SettingsLocation
    {
        get { return Path.Join(BaseDirectory,"Settings.json"); }
    }
}