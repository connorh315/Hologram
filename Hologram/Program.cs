using Hologram.Rendering;
using Hologram.FileTypes.GSC;
using Hologram.Objects;
using Hologram.FileTypes;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZACOPY.DNO", 0x1ca).PhysicsMesh;
            Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\HUB\VORTON\VORTON_TERR.DNO", 0x149).PhysicsMesh;


            //Mesh mesh = GSCFile.Read(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC");
            //Mesh mesh = GSCFile.Read(@"A:\Dimensions\game\LEVELS\BUILDER\TESTCANDY\TESTCANDYJL\TESTCANDYJL_NXG.GSC");

            using (MainWindow window = new MainWindow(mesh))
            {
                window.Run();
            }
        }
    }
}