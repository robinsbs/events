using Autofac;
using System;
using System.Collections.Generic;

namespace SkyBlueSoftware.Events.Autofac
{
    public static class SkyBlueSoftwareEvents
    {
        public static T InitializeApp<T>() => RegisterAllTypes<T>().Build().InitializeEvents().Resolve<T>();
        public static IContainer Initialize<T, TNew>(object instance) => RegisterAllTypes(instance, x => x.Is<T>(), x => x.Is<TNew>()).Build().InitializeEvents();
        public static IContainer Initialize(object instance, Func<Type, bool> typeSelector = null, Func<Type, bool> newInstanceSelector = null) => RegisterAllTypes(instance, typeSelector, newInstanceSelector).Build().InitializeEvents();
        public static IContainer Initialize<T>(object instance) => RegisterAllTypes(instance, x => x.Is<T>()).Build().InitializeEvents();
        public static ContainerBuilder RegisterAllTypes(object instance, Func<Type, bool> typeSelector = null, Func<Type, bool> newInstanceSelector = null) => C().RegisterAllTypes(instance, typeSelector, newInstanceSelector);
        public static ContainerBuilder RegisterAllTypes<T>() => C().RegisterAllTypes<T>();
        public static ContainerBuilder RegisterAllTypes(params Type[] allTypes) => C().RegisterAllTypes(allTypes);
        public static ContainerBuilder RegisterAllTypes(IEnumerable<Type> allTypes) => C().RegisterAllTypes(allTypes);

        private static ContainerBuilder C() => new ContainerBuilder();
    }
}
