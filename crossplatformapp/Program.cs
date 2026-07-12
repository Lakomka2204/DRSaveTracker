using Avalonia;
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
        Console.WriteLine("START APP");
        BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);
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
