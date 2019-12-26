using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SkyBlueSoftware.Events.ViewModel;

namespace SkyBlueSoftware.Events.View
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow
            {
                DataContext = new ContainerBuilder().RegisterAllTypes(this)
                                                                 .Build()
                                                                 .InitializeEvents()
                                                                 .Resolve<Main>()
            }.Show();
        }
    }

    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, params object[] instances) => RegisterAllTypes(b, instances.SelectMany(x => x.GetType().Assembly.GetTypes()));
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, params Type[] allTypes) => RegisterAllTypes(b, allTypes);
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, IEnumerable<Type> allTypes)
        {
            var types = allTypes.Where(x => x.IsAssignableTo<IRequireRegistration>())
                                .Union(new[] { typeof(EventStream), typeof(AutofacDependencyContainer) })
                                .Select(x => new AutofacTypeRegistrationDefinition(x, x.IsAssignableTo<IRequireRegistrationNew>()))
                                .ToArray();
            b.RegisterTypes(types.AsNew()).AsImplementedInterfaces().AsSelf();
            b.RegisterTypes(types.AsSingleInstance()).AsImplementedInterfaces().AsSelf().SingleInstance();
            b.RegisterContainer();
            return b;
        }

        public static IContainer InitializeEvents(this IContainer c)
        {
            c.Resolve<EventStream>().Initialize(c.Resolve<IEnumerable<ISubscribeTo>>());
            return c;
        }

        public static Type[] AsSingleInstance(this IEnumerable<AutofacTypeRegistrationDefinition> types)
        {
            return types.RegisterAs(x => x.RegisterAsSingleInstance);
        }

        public static Type[] AsNew(this IEnumerable<AutofacTypeRegistrationDefinition> types)
        {
            return types.RegisterAs(x => x.RegisterAsNew);
        }

        public static Type[] RegisterAs(this IEnumerable<AutofacTypeRegistrationDefinition> types, Func<AutofacTypeRegistrationDefinition, bool> selector)
        {
            return types.Where(selector).Select(x => x.Type).ToArray();
        }

        public static void RegisterContainer(this ContainerBuilder b)
        {
            IContainer container = null;
            b.Register(c => container).AsSelf();
            b.RegisterBuildCallback(c => container = c);
        }
    }

    public class AutofacTypeRegistrationDefinition
    {
        public AutofacTypeRegistrationDefinition(Type type, bool registerAsNew)
        {
            Type = type;
            RegisterAsNew = registerAsNew;
        }

        public Type Type { get; }
        public bool RegisterAsNew { get; }
        public bool RegisterAsSingleInstance => !RegisterAsNew;
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
}
