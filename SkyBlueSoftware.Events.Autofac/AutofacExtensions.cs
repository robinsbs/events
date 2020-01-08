using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SkyBlueSoftware.Events.Autofac
{
    public static class AutofacExtensions
    {
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, object instance) => RegisterAllTypes(b, instance.GetType().Assembly.GetTypes(), x => false, x => true);
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, object instance, Func<Type, bool> typeSelector) => RegisterAllTypes(b, instance.GetType().Assembly.GetTypes(), typeSelector, x => false);
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, object instance, Func<Type, bool> typeSelector, Func<Type, bool> newInstanceSelector) => RegisterAllTypes(b, instance.GetType().Assembly.GetTypes(), typeSelector, newInstanceSelector);
        public static ContainerBuilder RegisterAllTypes<T>(this ContainerBuilder b, Func<Type, bool> typeSelector, Func<Type, bool> newInstanceSelector) => RegisterAllTypes(b, typeof(T).Assembly.GetTypes(), typeSelector, newInstanceSelector);
        public static ContainerBuilder RegisterAllTypes<T>(this ContainerBuilder b) => RegisterAllTypes(b, typeof(T).Assembly.GetTypes(), x => false, x => true);
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, params Type[] allTypes) => RegisterAllTypes(b, allTypes.AsEnumerable(), x => x.Is(typeof(IRequireRegistration)), x => x.Is(typeof(IRequireRegistrationNew)));
        public static ContainerBuilder RegisterAllTypes(this ContainerBuilder b, IEnumerable<Type> allTypes, Func<Type, bool> typeSelectorSingleton, Func<Type, bool> typeSelectorNewInstance)
        {
            var types = allTypes.Where(x => typeSelectorSingleton(x) || typeSelectorNewInstance(x))
                                .Union(new[] { typeof(EventStream), typeof(AutofacDependencyContainer) })
                                .Select(x => new AutofacTypeRegistrationDefinition(x, typeSelectorNewInstance(x)))
                                .ToArray();
            b.RegisterTypes(types.AsNew()).AsImplementedInterfaces().AsSelf();
            b.RegisterTypes(types.AsSingleInstance()).AsImplementedInterfaces().AsSelf().SingleInstance();
            b.RegisterContainer();
            return b;
        }

        public static bool IsAssignableTo<T>(this Type source) => IsAssignableTo(source, typeof(T));
        public static bool IsAssignableTo(this Type source, Type target) => target.GetTypeInfo().IsAssignableFrom(source.GetTypeInfo());

        public static bool Is(this Type source, Type target) => source.IsAssignableTo(target);
        public static bool Is<T>(this Type t) => t.IsAssignableTo<T>();
        public static bool Is<T1, T2>(this Type t) => t.Is<T1>() || t.Is<T2>();
        public static bool Is<T1, T2, T3>(this Type t) => t.Is<T1, T2>() || t.Is<T3>();
        public static bool Is<T1, T2, T3, T4>(this Type t) => t.Is<T1, T2, T3>() || t.Is<T4>();
        public static bool Is<T1, T2, T3, T4, T5>(this Type t) => t.Is<T1, T2, T3, T4>() || t.Is<T5>();
        public static bool Is<T1, T2, T3, T4, T5, T6>(this Type t) => t.Is<T1, T2, T3, T4, T5>() || t.Is<T6>();
        public static bool Is<T1, T2, T3, T4, T5, T6, T7>(this Type t) => t.Is<T1, T2, T3, T4, T5, T6>() || t.Is<T7>();
        public static bool Is<T1, T2, T3, T4, T5, T6, T7, T8>(this Type t) => t.Is<T1, T2, T3, T4, T5, T6, T7>() || t.Is<T8>();
        public static bool Is<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Type t) => t.Is<T1, T2, T3, T4, T5, T6, T7, T8>() || t.Is<T9>();
        public static bool Is<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Type t) => t.Is<T1, T2, T3, T4, T5, T6, T7, T8, T9>() || t.Is<T10>();

        public static IContainer InitializeEvents(this IContainer c)
        {
            c.Resolve<EventStream>().Initialize(c.Resolve<IEnumerable<ISubscribeTo>>());
            return c;
        }

        public static (T1, T2) Resolve<T1, T2>(this IContainer c) => (c.Resolve<T1>(), c.Resolve<T2>());
        public static (T1, T2, T3) Resolve<T1, T2, T3>(this IContainer c) => (c.Resolve<T1>(), c.Resolve<T2>(), c.Resolve<T3>());

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
