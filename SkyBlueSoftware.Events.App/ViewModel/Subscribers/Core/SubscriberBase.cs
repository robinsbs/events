using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.ViewModel
{
    public abstract class SubscriberBase
    {
        private int counter;

        public SubscriberBase()
        {
            Log = new ObservableCollection<string>();
            counter = 0;
        }

        public string Name => GetType().Name;
        public ObservableCollection<string> Log { get; }

        protected async Task LogEvent<T>(T e)
        {
            Log.Insert(0, $"{++counter} - Received event {e?.GetType().Name}");
            await Task.CompletedTask;
        }
    }
}
