using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public class B : ISubscribeTo<Event2>
    {
        public B()
        {
            Log = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Log { get; }

        public async Task On(Event2 e)
        {
            Log.Insert(0, $"Received event {e}");
            await Task.CompletedTask;
        }
    }
}
