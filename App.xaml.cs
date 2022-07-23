using System.Windows;

namespace ToolDemocraci
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow = new MainWindowView(new ViewModel.MainViewModel());
            MainWindow.Show();
        }
    }
}