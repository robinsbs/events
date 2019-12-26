using System;

namespace SkyBlueSoftware.Events.Autofac
{
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
}
