using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace crossplatformapp;

public partial class MainWindow : Window
{
    private MainWindowViewModel model;
    public MainWindow()
    {
        InitializeComponent();
        model = new MainWindowViewModel(this);
        DataContext = model;
    }
    protected override async void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        await model.Initialize();
    }


}