using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace crossplatformapp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var model = new MainWindowViewModel();
        DataContext = model;
    }

}