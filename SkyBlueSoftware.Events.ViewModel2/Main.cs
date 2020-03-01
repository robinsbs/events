using SkyBlueSoftware.Events.Autofac;

namespace SkyBlueSoftware.Events.ViewModel
{
    public class Main : ViewModelBase, IRequireRegistration
    {
        public Main(Body body)
        {
            Body = body;
        }

        public string Title => "Events Application";
        public Body Body { get; }
    }
}
