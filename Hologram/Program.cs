using Hologram.Rendering;
using Hologram.Objects;
using ModLib;

using OpenTK.Mathematics;
using Hologram.FileTypes.DNO;
using Hologram.FileTypes.GSC;
using Hologram.FileTypes;
using Hologram.FileTypes.DDS;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                GSC test = GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\TARDIS\TARDIS11\TARDIS11_NXG.GSC");
                //DDS.Load(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG\LEGO_Zeus\LEGO_Zeus_Images_Nut\brick\nd_yellowbrickroad_nostud_diff.nut.dds");
                //MeshX mesh = test.ConvertPart(test.parts[0]);
                //window.AddMesh(mesh, true);
                window.AddEntities(test.entities);

                window.Run();
            }
        }
    }
}