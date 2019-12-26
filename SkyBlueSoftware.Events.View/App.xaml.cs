using Autofac;
using System.Windows;
using SkyBlueSoftware.Events.ViewModel;
using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.View
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow
            {
                DataContext = new ContainerBuilder().RegisterAllTypes<Main>()
                                                    .Build()
                                                    .InitializeEvents()
                                                    .Resolve<Main>()
            }.Show();
        }
    }
}
