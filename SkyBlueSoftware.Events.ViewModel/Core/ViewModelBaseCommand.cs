// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class ViewModelBaseCommand : ICommand
    {
        private readonly Action action;

        public ViewModelBaseCommand(Action action)
        {
            this.action = action;
        }

        public event EventHandler CanExecuteChanged = (o, e) => { };
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => action();
    }
}
