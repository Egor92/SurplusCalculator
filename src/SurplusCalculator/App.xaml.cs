using System;
using System.Windows;
using System.Windows.Media.Imaging;
using SurplusCalculator.Infrastructure;
using SurplusCalculator.Models;
using SurplusCalculator.ViewModels;
using SurplusCalculator.Views;

namespace SurplusCalculator
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var fileSelector = new FileSelector();
            var surplusCalculator = new ApproximateSurplusCalculator();
            var window = new Window
            {
                Content = new MainView
                {
                    DataContext = new MainViewModel(fileSelector, surplusCalculator),
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
