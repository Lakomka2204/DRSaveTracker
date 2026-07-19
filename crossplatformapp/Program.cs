using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;

namespace crossplatformapp;

class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    public static string AppName = "DRSaveTracker";
    [STAThread]
    public static void Main(string[] args)
    {
        Console.WriteLine("Main entry point start, args: {0}",string.Join(' ',args));
        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args, Avalonia.Controls.ShutdownMode.OnExplicitShutdown);
        Console.WriteLine("Main entry point end");
    }
    public static IClassicDesktopStyleApplicationLifetime? GetLife()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        return desktop;
        else return null;
    }
    // Avalonia configuration, don't remove; also used by visual designer.

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
#if DEBUG
            .WithDeveloperTools()
#endif
            // .WithInterFont()
            .ConfigureFonts(fm =>
            {
                fm.AddFontCollection(new DRFontCollection());
            })
            .LogToTrace();
}
