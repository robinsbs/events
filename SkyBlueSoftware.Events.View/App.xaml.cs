// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Windows;
using SkyBlueSoftware.Events.ViewModel;
using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.View
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            new MainWindow { DataContext = SkyBlueSoftwareEvents.InitializeApp<Main, IRequireRegistration, IRequireRegistrationNew>() }.Show();
        }
    }
}
