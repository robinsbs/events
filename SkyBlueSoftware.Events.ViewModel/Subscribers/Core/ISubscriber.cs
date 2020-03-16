// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using SkyBlueSoftware.Events.Autofac;
using System.Collections.ObjectModel;

namespace SkyBlueSoftware.Events.ViewModel
{
    public interface ISubscriber : IRequireRegistration
    {
        string Delay { get; set; }
        ObservableCollection<string> Log { get; }
        string Name { get; }
    }
}