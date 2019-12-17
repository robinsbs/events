using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public abstract class SubscriberBase
    {
        public SubscriberBase()
        {
            Log = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Log { get; }

        protected async Task LogEvent<T>(T e)
        {
            Log.Insert(0, $"Received event {e}");
            await Task.CompletedTask;
        }
    }
}
