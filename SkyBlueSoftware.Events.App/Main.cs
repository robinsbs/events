namespace SkyBlueSoftware.Events.App
{
    public class Main
    {
        public Main(Body body)
        {
            Body = body;
        }

        public string Title => "Events Application";
        public Body Body { get; }
    }
}
