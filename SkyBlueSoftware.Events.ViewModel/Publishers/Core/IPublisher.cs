// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using SkyBlueSoftware.Events.Autofac;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public interface IPublisher : IRequireRegistration
    {
        string Label { get; }
        string Name { get; }
        ICommand PublishCommand { get; }
    }
}