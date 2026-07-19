

using System;
using System.ComponentModel;
using System.Runtime.Versioning;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;

namespace crossplatformapp.Utils.Startup;

[SupportedOSPlatform("Windows")]
public partial class WindowsStartup : ObservableObject, IStartup
{
    public WindowsStartup()
    {
        Status = Check();
    }
    [ObservableProperty]
    private StartupStatus status;
    private static readonly string AutoRunPath = 
        @"Software\Microsoft\Windows\CurrentVersion\Run";
    private static readonly string ApprovePath =
        @"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

    public static StartupStatus Check()
    {
        using var autorunKey = 
            Registry.CurrentUser.OpenSubKey(AutoRunPath);
        if (autorunKey?.GetValue(Program.AppName) == null)
            return StartupStatus.Unregistered;
        
        using var approveKey = 
            Registry.CurrentUser.OpenSubKey(ApprovePath);
        if (approveKey is null)
            return StartupStatus.Registered;
        if (approveKey?.GetValue(Program.AppName) is not byte[] value)
            return StartupStatus.Registered;
        return value[0] == 0x02 ? StartupStatus.Registered : StartupStatus.DisabledByUser;
    }
    public void Register()
    {
        using var key = 
            Registry.CurrentUser.CreateSubKey(AutoRunPath,true);
        key?.SetValue(
            Program.AppName,
            $"\"{Environment.ProcessPath}\" -s");
        Status = Check();
    }
    public void Unregister()
    {
        using var key = 
            Registry.CurrentUser.OpenSubKey(AutoRunPath,true);
        key?.DeleteValue(Program.AppName);
        Status = Check();
    }
}