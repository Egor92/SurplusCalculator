using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SurplusCalculator.Infrastructure;
using SurplusCalculator.ViewModels;
using SurplusCalculator.Views;

namespace SurplusCalculator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var window = new Window
            {
                Content = new MainView
                {
                    DataContext = new MainViewModel(new FileSelector()),
                },
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                Width = 300,
                Height = 300,
                Title = "Surplus calculator",
                Icon = new BitmapImage(new Uri("icon.ico", UriKind.Relative)),
            };
            window.Show();
        }
    }
}
