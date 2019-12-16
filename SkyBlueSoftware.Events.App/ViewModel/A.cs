using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SkyBlueSoftware.Events.App
{
    public class A : ISubscribeTo<Event1>
    {
        public A()
        {
            Log = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Log { get; }

        public async Task On(Event1 e)
        {
            Log.Insert(0, $"Received event {e}");
            await Task.CompletedTask;
        }
    }
}
