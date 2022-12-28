using Hologram.Rendering;
using Hologram.Objects;
using ModLib;

using OpenTK.Mathematics;
using Hologram.FileTypes.DNO;
using Hologram.FileTypes.GSC;
using Hologram.FileTypes;
using Hologram.FileTypes.DDS;
using System.Text;
using Hologram.FileTypes.DAE;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                GSC test = GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZB\1WIZARDOFOZB_NXG.GSC");
                DAE.Create("C:/users/connor/Desktop/DAE/test.dae", test.entities);

                window.AddEntities(test.entities);

                window.Run();
            }
        }
    }
}