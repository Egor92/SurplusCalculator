using System.Windows;
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
                Width = 600,
                Height = 500,
                Title = "Surplus calculator"
            };
            window.Show();
        }
    }
}
