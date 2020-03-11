using SkyBlueSoftware.Storage.Test;

namespace SkyBlueSoftware.TestFramework.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var test = new Storage_Tests();
            test.Initialize();
            test.Storage_Tests_SqlServer();
        }
    }
}
