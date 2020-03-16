// Licensed to Sky Blue Software under one or more agreements.
// Sky Blue Software licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class SubscriberBase : ISubscriber
    {
        private int counter;

        protected SubscriberBase()
        {
            Log = new ObservableCollection<string>();
            counter = 0;
            Delay = "0";
        }

        public string Name => GetType().Name;
        public string Delay { get; set; }
        public ObservableCollection<string> Log { get; }

        protected async Task LogEvent<T>(T e)
        {
            if (int.TryParse(Delay, out var delay)) await Task.Delay(delay);
            Log.Insert(0, $"{++counter} - Received event {e?.GetType().Name}");
        }
    }
}
