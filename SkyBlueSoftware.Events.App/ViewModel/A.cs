using System.Collections.ObjectModel;

namespace SkyBlueSoftware.Events.App
{
    public class A : ISubscribeToSync<Event1>
    {
        public A()
        {
            Log = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Log { get; }

        public void On(Event1 e)
        {
            Log.Insert(0, $"Received event {e}");
        }
    }
}
