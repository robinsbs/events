using Autofac;
using SkyBlueSoftware.Events.ViewModel;
using System.Collections.Generic;
using System.Windows;

namespace SkyBlueSoftware.Events.View
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var b = new ContainerBuilder();
            //b.RegisterEvents();
            b.RegisterType<EventStream>().SingleInstance().AsImplementedInterfaces().AsSelf();
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
            c.Resolve<EventStream>().Initialize(c.Resolve<IEnumerable<ISubscribeTo>>());
            //c.InitializeEvents();
            var mainWindow = new MainWindow();
            mainWindow.DataContext = c.Resolve<Main>();
            mainWindow.Show();
        }
    }
}
