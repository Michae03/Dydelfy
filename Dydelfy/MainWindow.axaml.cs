using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Dydelfy;

public partial class MainWindow : Window
{
    private Settings settings;
    public MainWindow()
    {
        InitializeComponent();
        settings = new Settings();
    }

    private async void Settings_On_Click(object? sender, RoutedEventArgs e)
    {
        settings = new Settings();
        await settings.ShowDialog(this);
        Console.Out.WriteLine(settings.time);
    }

    private void Start_On_Click(object? sender, RoutedEventArgs e)
    {
        GameWindow window = new GameWindow(settings);
        window.Show();
    }
}