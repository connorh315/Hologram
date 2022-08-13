using Hologram.Rendering;
using Hologram.FileTypes.GSC;
using Hologram.Objects;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //DNO dnoFile = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_TER.DNO", 0x151);

            Mesh mesh = GSCFile.Read(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC");

            using (MainWindow window = new MainWindow(mesh))
            {
                window.Run();
            }
        }
    }
}