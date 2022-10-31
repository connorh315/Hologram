using Hologram.Rendering;
using Hologram.Objects;
using ModLib;

using OpenTK.Mathematics;
using Hologram.FileTypes.DNO;
using Hologram.FileTypes.GSC;
using Hologram.FileTypes;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                GSC test = GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC");
                MeshX mesh = test.ConvertPart(test.parts[0]);
                window.AddMesh(mesh, true);

                window.Run();
            }
        }
    }
}