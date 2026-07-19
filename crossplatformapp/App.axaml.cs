using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;

namespace crossplatformapp;

public partial class App : Application
{
    public override void Initialize()
    {
        DataContext = this;
        System.Console.WriteLine(typeof(App).Assembly.GetName().Name);
        AvaloniaXamlLoader.Load(this);
        if (Current != null)
            Current.Dispatcher.UnhandledException 
                += UnhandledException;
    }
    private Window? _window;
    private TrayIcon? _trayIcon;
    private async void UnhandledException(object sender,DispatcherUnhandledExceptionEventArgs args)
    {
        var box = MessageBoxes.GetOwnMBox("Critical error",args.Exception.Message, MsBox.Avalonia.Enums.Icon.Error);
        await box.ShowWindowAsync();
        EndWindow();
    }
    private void WindowClosing(object? sender, WindowClosingEventArgs args)
    {
        // if (_reallyExit)
        //     return;
        System.Console.WriteLine("close reason {0} programaticaly {1}",args.CloseReason,args.IsProgrammatic);
        if (args.CloseReason == WindowCloseReason.ApplicationShutdown)
            return;
        args.Cancel = true;
        HideWindow();
    }
    [RelayCommand]
    private void HideWindow()
    {
        _window?.Hide();
    }
    [RelayCommand]
    private void ShowWindow()
    {
        var desktop = Program.GetLife();
        if (desktop?.MainWindow is null)
            desktop!.MainWindow = _window;
        _window?.Show();
        _window?.Activate();
        _window!.WindowState = WindowState.Normal;
    }
    [RelayCommand]
    private static void EndWindow()
    {
        Program.GetLife()?.Shutdown(0);
        // _reallyExit = true;
        // _window?.Close();
    }
    private void ToggleTrayIcon(object? sender,AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Property != Visual.IsVisibleProperty)
            return;
        if (sender is not Window window)
            return;
        if (_trayIcon == null)
            return;
        _trayIcon.IsVisible = !window.IsVisible;
    }
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            _window = new MainWindow();
            _window.Closing += WindowClosing;
            _window.PropertyChanged += ToggleTrayIcon;
            
            bool silent = desktop.Args is not null &&
                Array.Exists(desktop.Args,f => f is "-s");
            _trayIcon = TrayIcon.GetIcons(this)?.FirstOrDefault();
            if (silent)
            {
                _trayIcon!.IsVisible = true;
            }
            else
            {
            desktop.MainWindow = _window;
                
            }
        }

        base.OnFrameworkInitializationCompleted();
    }
}