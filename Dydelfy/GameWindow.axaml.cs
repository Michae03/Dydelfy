using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;

namespace Dydelfy;

public partial class GameWindow : Window
{
    private List<char> elements;
    private Settings settings;
    private int d_found;
    private DispatcherTimer timer;
    private int gameTime;
    private DispatcherTimer crocTimer;
    private bool crocRevealed;
    public GameWindow()
    {
        InitializeComponent();
    }

    public GameWindow(Settings set)
    {
        InitializeComponent();
        settings = set;
        elements = randomisedElements();
        d_found = 0;
        CreateBoard(settings.board.X, settings.board.Y);
        crocRevealed = false;
        gameTime = settings.time;
        TimeLabel.Content = gameTime.ToString();
        initTimer();
    }

    private void initTimer()
    {
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += (sender, e) =>
        {
            gameTime--;
            TimeLabel.Content = gameTime.ToString();
            if (gameTime == 0)
            {
               gameOver();
            }
        };
        timer.Start();
    }

    private void beginCrocTimer()
    {
        int doom = 2;
        crocTimer = new DispatcherTimer();
        crocTimer.Interval = TimeSpan.FromSeconds(1);
        crocTimer.Tick += (sender, e) =>
        {
            doom--;
            if (doom == 0)
            {
                gameOver();
            }
           
        };
        crocTimer.Start();
    }

    private void gameOver()
    {
        foreach (var button in Plansza.Children)
        {
            button.IsEnabled = false;
        }
        TimeLabel.Content = "PRZEGRALES :(";
        timer.Stop();
    }

    public void CreateBoard(float Width, float Height)
    {
        for (int i = 0; i < Width; i++)
            Plansza.RowDefinitions.Add(new RowDefinition());

        for (int j = 0; j < Height; j++)
            Plansza.ColumnDefinitions.Add(new ColumnDefinition());
        int e = -1;
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                e++;
                var button = new Button
                {
                    Content = $"\ud83d\uddd1",
                    Tag = $"{elements[e]}",
                    Height = 45,
                    Width = 45,
                    FontSize = 22
                };
                button.Click += On_Click_Trashcan;
                Grid.SetRow(button, i);
                Grid.SetColumn(button, j);

                Plansza.Children.Add(button);
            }
        }
    }

    public List<Char> randomisedElements()
    {
        List<char> animals = new();
        for (int i = 0; i < settings.krokodyle; i++)
        {
            animals.Add('k');
        }

        for (int i = 0; i < settings.dydelfy; i++)
        {
            animals.Add('d');
        }

        for (int i = 0; i < settings.szopy; i++)
        {
            animals.Add('s');
        }
        float rest = settings.board.X * settings.board.Y - animals.Count;
        for (int i = 0; i < rest; i++)
        {
            animals.Add('p');
        }
        Random rng = new Random();
        for (int i = animals.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (animals[i], animals[j]) = (animals[j], animals[i]);
        }

        return animals;
    }

    private Button? GetButtonAt(int row, int column)
    {
        foreach (var child in Plansza.Children)
        {
            if (child is Button btn &&
                Grid.GetRow(btn) == row &&
                Grid.GetColumn(btn) == column)
            {
                return btn;
            }
        }
        return null;
    }
    private void On_Click_Trashcan(object? sender, RoutedEventArgs e)
    {
        if (sender is Button trashcan)
        {
            var animal = trashcan.Tag?.ToString();
            
            switch (animal)
            {
                case "k":
                    if (!crocRevealed)
                    {
                        trashcan.Content = "🐊";
                        crocRevealed = true;
                        beginCrocTimer();
                    }
                    else
                    {
                        trashcan.Content = "\ud83d\uddd1";
                        crocTimer.Stop();
                        crocRevealed = false;
                    }
                    
                    
                    break;
                case "d":
                    trashcan.Content = "\ud83d\udc7e";
                    d_found++;
                    if (d_found == settings.dydelfy)
                    {
                        TimeLabel.Content = "WYGRALES :)";
                        foreach (var button in Plansza.Children)
                        {
                            button.IsEnabled = false;
                        }
                        timer.Stop();
                    }
                    break;
                case "s":
                    trashcan.Content = "🦝"; 
                    DispatcherTimer szoptimer = new DispatcherTimer();
                    int szopTime = 2;
                    szoptimer.Interval = TimeSpan.FromSeconds(1);
                    szoptimer.Tick += (sender, e) =>
                    {
                        szopTime--;
                        if (szopTime == 0)
                        {
                            int x = Grid.GetRow(trashcan);
                            int y = Grid.GetColumn(trashcan);
                            szoptimer.Stop();
                            trashcan.Content = "\ud83d\uddd1";
                            if (x > 0)
                            {
                                GetButtonAt(x-1, y).Content = "\ud83d\uddd1";
                                GetButtonAt(x-1, y).IsEnabled = true;
                            }

                            if (y > 0)
                            {
                                GetButtonAt(x, y - 1).Content = "\ud83d\uddd1";
                                GetButtonAt(x, y - 1).IsEnabled = true;
                            }

                            if (y < settings.board.Y)
                            {
                                GetButtonAt(x, y + 1).Content = "\ud83d\uddd1";
                                GetButtonAt(x, y + 1).IsEnabled = true;
                            }

                            if (x < settings.board.X)
                            {
                                GetButtonAt(x + 1, y).Content = "\ud83d\uddd1";
                                GetButtonAt(x + 1, y).IsEnabled = true;
                            }
                            
                        }
                    };
                    szoptimer.Start();
                    break;
                case "p":
                    trashcan.Content = "";
                    trashcan.IsEnabled = false;
                    break;
            }
        }
    }
}