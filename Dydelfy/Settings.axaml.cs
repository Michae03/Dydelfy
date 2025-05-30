using System.Numerics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Dydelfy;

public partial class Settings : Window
{
    public Vector2 board = Vector2.Zero;
    public int dydelfy;
    public int szopy;
    public int krokodyle;
    public int time;
    
    
    public Settings()
    {
        InitializeComponent();
        board.X = 3;
        board.Y = 3;
        krokodyle = 0;
        szopy = 3;
        dydelfy = 1;
        time = 10;
    }

    private void Confirm_OnClick(object? sender, RoutedEventArgs e)
    {
        board.X = (int.TryParse(x_value_box.Text, out int x) && x >= 3 && x <= 10) ? x : 3;
        board.Y = (int.TryParse(y_value_box.Text, out int y) && y >= 3 && y <= 10) ? y : 3;
        dydelfy = (int.TryParse(dydelfy_box.Text, out int dy) && dy >= 1 && dy <= 6) ? dy : 1;
        szopy = (int.TryParse(szopy_box.Text, out int sz) && sz >= 3 && sz <= 8) ? sz : 3;
        krokodyle = int.TryParse(krokodyle_box.Text, out int kr) && kr >= 0 && kr <= 1 ? kr : 0;
        time = int.TryParse(time_box.Text, out int t) && t >= 10 && t <= 60 ? t : 10;
        Close();
    }
}