using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SkyBlueSoftware.Events.Autofac
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterAllTypes<T>(this ContainerBuilder b) => RegisterAllTypes(b, typeof(T).Assembly.GetTypes());
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, params Type[] allTypes) => RegisterAllTypes(b, allTypes.AsEnumerable());
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
}
