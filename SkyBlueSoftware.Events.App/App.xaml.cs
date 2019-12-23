using Autofac;
using SkyBlueSoftware.Events.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            b.RegisterType<AutofacDependencyContainer>().As<IDependencyContainer>().SingleInstance();
            b.RegisterType<Factory>().As<IFactory>().SingleInstance();
            b.RegisterType<Event1>();
            b.RegisterType<Event2>();
            b.RegisterType<Event3>();
            b.RegisterType<Event4>();
            b.RegisterContainer();
            var c = b.Build();
            c.Resolve<EventStream>().Initialize(c.Resolve<IEnumerable<ISubscribeTo>>());
            //c.InitializeEvents();
            var mainWindow = new MainWindow();
            mainWindow.DataContext = c.Resolve<Main>();
            mainWindow.Show();
        }
    }

    public class AutofacDependencyContainer : IDependencyContainer
    {
        private readonly IContainer container;

        public AutofacDependencyContainer(IContainer container)
        {
            this.container = container;
        }
        public Task<T> Create<T>(params object[] args)
        {
            var parameters = args.Where(x => x != null).Select(x => new TypedParameter(x.GetType(), x)).ToArray();
            var o = container.Resolve<T>(parameters);
            return Task.FromResult(o);
        }
    }

    public static class AutofacExtensions
    {
        public static void RegisterContainer(this ContainerBuilder b)
        {
            IContainer? container = null;
            b.Register(c => container).AsSelf();
            b.RegisterBuildCallback(c => container = c);
        }
    }
}
