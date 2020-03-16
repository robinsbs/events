// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
