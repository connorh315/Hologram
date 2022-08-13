using Hologram.Rendering;
using Hologram.FileTypes;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DNO dnoFile = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_TER.DNO", 0x151);

            using (MainWindow window = new MainWindow(dnoFile.PhysicsMesh))
            {
                window.Run();
            }
        }
    }
}