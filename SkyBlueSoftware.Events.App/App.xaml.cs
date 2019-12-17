using Autofac;
using System.Windows;

namespace SkyBlueSoftware.Events.App
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var b = new ContainerBuilder();
            b.RegisterType<EventStream>().SingleInstance();
            b.RegisterAssemblyTypes(typeof(SubscriberBase).Assembly)
                .Where(t => t.IsSubclassOf(typeof(SubscriberBase)))
                .SingleInstance()
                .AsImplementedInterfaces()
                .As<SubscriberBase>();
            b.RegisterAssemblyTypes(typeof(PublisherBase).Assembly)
                .Where(t => t.IsSubclassOf(typeof(PublisherBase)))
                .SingleInstance()
                .AsImplementedInterfaces()
                .As<PublisherBase>();
            b.RegisterType<Body>().SingleInstance();
            b.RegisterType<Main>().SingleInstance();
            var c = b.Build();
            var mainWindow = new MainWindow();
            mainWindow.DataContext = c.Resolve<Main>();
            mainWindow.Show();
        }
    }
}
