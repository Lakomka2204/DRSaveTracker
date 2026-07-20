




using System;
using CommunityToolkit.Mvvm.ComponentModel;
using crossplatformapp.Utils;
using crossplatformapp.Utils.Startup;
namespace crossplatformapp;

public partial class SettingsViewModel : ObservableObject
{
    public IStartup Startup {get; private set;}
    public Settings Settings {get; private set;}
    public SettingsViewModel()
    {
        Startup = StartupFactory.Create();
        Settings = Settings.Current;
    }
    public bool IsStartupLocked
    {
        get => Startup.Status == StartupStatus.DisabledByUser;
    }
    public bool IsStartupEnabled
    {
        get => Startup.Status != StartupStatus.Unregistered;
        set
        {
            System.Console.WriteLine("prop changed value {0} Startup {1}",value,Startup.Status);
            
            if (value)
                Startup.Register();
            else
                Startup.Unregister();
            OnPropertyChanged(nameof(IsStartupEnabled));
        }
    }
}