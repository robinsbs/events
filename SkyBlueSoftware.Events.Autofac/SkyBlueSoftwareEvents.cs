// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Autofac;
using System;
using System.Collections.Generic;

namespace SkyBlueSoftware.Events.Autofac
{
    public static class SkyBlueSoftwareEvents
    {
        public static TApp InitializeApp<TApp>() => RegisterAllTypes<TApp>().Build().InitializeEvents().Resolve<TApp>();
        public static (TApp, TInstance) InitializeApp<TApp, TInstance>() => RegisterAllTypes<TApp>().Build().InitializeEvents().Resolve<TApp, TInstance>();
        public static TApp InitializeApp<TApp, TSingleton, TNew>() => RegisterAllTypes<TApp>(x => x.Is<TSingleton>(), x => x.Is<TNew>()).Build().InitializeEvents().Resolve<TApp>();
        public static (TApp, TInstance) InitializeApp<TApp, TSingleton, TNew, TInstance>() => RegisterAllTypes<TApp>(x => x.Is<TSingleton>(), x => x.Is<TNew>()).Build().InitializeEvents().Resolve<TApp, TInstance>();
        public static IContainer Initialize<TSingleton, TNew>(object instance) => RegisterAllTypes(instance, x => x.Is<TSingleton>(), x => x.Is<TNew>()).Build().InitializeEvents();
        public static IContainer Initialize(object instance, Func<Type, bool> typeSelector = null, Func<Type, bool> newInstanceSelector = null) => RegisterAllTypes(instance, typeSelector, newInstanceSelector).Build().InitializeEvents();
        public static IContainer Initialize<T>(object instance) => RegisterAllTypes(instance, x => x.Is<T>()).Build().InitializeEvents();
        public static ContainerBuilder RegisterAllTypes(object instance, Func<Type, bool> typeSelector) => C().RegisterAllTypes(instance, typeSelector);
        public static ContainerBuilder RegisterAllTypes(object instance, Func<Type, bool> typeSelector, Func<Type, bool> newInstanceSelector) => C().RegisterAllTypes(instance, typeSelector, newInstanceSelector);
        public static ContainerBuilder RegisterAllTypes<T>(Func<Type, bool> typeSelector, Func<Type, bool> newInstanceSelector) => C().RegisterAllTypes<T>(typeSelector, newInstanceSelector);
        public static ContainerBuilder RegisterAllTypes<T>() => C().RegisterAllTypes<T>();
        public static ContainerBuilder RegisterAllTypes(params Type[] allTypes) => C().RegisterAllTypes(allTypes);
        public static ContainerBuilder RegisterAllTypes(Func<Type, bool> typeSelector, Func<Type, bool> newInstanceSelector, params Type[] allTypes) => C().RegisterAllTypes(typeSelector, newInstanceSelector, allTypes);
        public static ContainerBuilder RegisterAllTypes(IEnumerable<Type> allTypes) => C().RegisterAllTypes(allTypes);

        private static ContainerBuilder C() => new ContainerBuilder();
    }
}
