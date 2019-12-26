using Autofac;
using System;
using System.Collections.Generic;

namespace SkyBlueSoftware.Events.Autofac
{
    public static class SkyBlueSoftwareEvents
    {
        public static ContainerBuilder RegisterAllTypes<T>() => C().RegisterAllTypes<T>();
        public static ContainerBuilder RegisterAllTypes(params Type[] allTypes) => C().RegisterAllTypes(allTypes);
        public static ContainerBuilder RegisterAllTypes(IEnumerable<Type> allTypes) => C().RegisterAllTypes(allTypes);

        private static ContainerBuilder C() => new ContainerBuilder();
    }
}
