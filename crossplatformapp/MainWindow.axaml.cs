using System;
using Avalonia.Controls;

namespace crossplatformapp;

public partial class MainWindow : Window
{
    private MainWindowViewModel model;
    public MainWindow()
    {
        model = new MainWindowViewModel(this);
        DataContext = model;
        InitializeComponent();
    }
    private bool initialized = false;
    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        if (!initialized)
        {
            await model.Initialize();
            initialized = true;
        }
    }
}