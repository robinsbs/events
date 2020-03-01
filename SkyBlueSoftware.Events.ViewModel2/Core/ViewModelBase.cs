using System;
using System.Windows.Input;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class ViewModelBase
    {
        public ICommand Do(Action action) => new ViewModelBaseCommand(action);
    }
}
