using Tizen.Applications;
using Uno.UI.Runtime.Skia;

namespace MyUnoApp2.Skia.Tizen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = new TizenHost(() => new MyUnoApp2.App());
            host.Run();
        }
    }
}
