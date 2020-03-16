// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class ViewModelBase
    {
        public ICommand Do(Action action) => new ViewModelBaseCommand(action);
    }
}
