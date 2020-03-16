// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class Main : ViewModelBase, IRequireRegistration
    {
        public Main(Body body)
        {
            Body = body;
        }

        public string Title => "Events Application";
        public Body Body { get; }
    }
}
