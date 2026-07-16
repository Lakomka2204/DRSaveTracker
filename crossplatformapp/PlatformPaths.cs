using System;
using System.IO;


namespace crossplatformapp;

public static class PlatformPaths
{
    private static readonly string GameName = "DELTARUNE";
    public static string SaveDirectory
    {
        get
        {
            if (OperatingSystem.IsWindows())
                return Path.Join(
            Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData),
            GameName);
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
}