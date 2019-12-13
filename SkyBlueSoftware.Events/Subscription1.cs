using System.Threading.Tasks;

namespace SkyBlueSoftware.Events
{
    public class Subscription<T> : Subscription
    {
        private readonly ISubscribeTo<T> subscriber;

        public Subscription(ISubscribeTo<T> subscriber) : base(subscriber.GetType(), typeof(T))
        {
            this.subscriber = subscriber;
        }

        public override async Task On(object o)
        {
            if (IsNotSubscribed) return;
            if (o is T e)
            {
                await subscriber.On(e);
                CallCount += 1;
            }
        }
    }
}