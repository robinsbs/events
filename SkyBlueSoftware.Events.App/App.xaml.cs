using Autofac;
using System.Windows;

namespace SkyBlueSoftware.Events.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var b = new ContainerBuilder();
            b.RegisterType<A>().SingleInstance().AsImplementedInterfaces().AsSelf();
            b.RegisterType<Body>().SingleInstance();
            b.RegisterType<Main>().SingleInstance();
            var c = b.Build();
            var mainWindow = new MainWindow();
            mainWindow.DataContext = c.Resolve<Main>();
            mainWindow.Show();
        }
    }
}
