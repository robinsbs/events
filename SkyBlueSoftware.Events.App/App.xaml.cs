using System.Windows;

namespace SkyBlueSoftware.Events.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow().Show();
        }
    }
}
