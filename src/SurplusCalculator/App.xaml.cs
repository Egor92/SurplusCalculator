using System;
using System.IO;
using System.Text;
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
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

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
        
        private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            const string timeFormat = @"yyyy-MM-dd hh-mm-ss";
            string timeString = DateTime.Now.ToString(timeFormat);
            string path = $@"{Directory.GetCurrentDirectory()}\logs\error ({timeString}).txt";

            var errorMessage = GetFullErrorMessage(e);
            File.WriteAllText(path, errorMessage);
            
            string message = @"Произошла необработанная ошибка. Приложение будет закрыто.";
            MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private static string GetFullErrorMessage(UnhandledExceptionEventArgs e)
        {
            StringBuilder errorMessageBuilder = new StringBuilder();
            var exception = (Exception) e.ExceptionObject;

            while (exception != null)
            {
                errorMessageBuilder.AppendLine(exception.Message);
                errorMessageBuilder.AppendLine();
                errorMessageBuilder.AppendLine(exception.StackTrace);
                errorMessageBuilder.AppendLine();
                errorMessageBuilder.AppendLine("========================================");
                errorMessageBuilder.AppendLine();

                exception = exception.InnerException;
            }

            return errorMessageBuilder.ToString();
        }
    }
}
