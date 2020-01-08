using System.Windows;
using SkyBlueSoftware.Events.ViewModel;
using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.View
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow { DataContext = SkyBlueSoftwareEvents.InitializeApp<Main>() }.Show();
        }
    }
}
