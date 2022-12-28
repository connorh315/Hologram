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
#if DEBUG
                GSC gscFile = GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\11SCOOBYDOO\11SCOOBYDOOA\11SCOOBYDOOA_NXG.GSC");
                DAE.Create("C:/users/connor/Desktop/DAE/test.dae", gscFile.entities);
#else
                GSC gscFile;
                if (args.Length == 0)
                {
                    Logger.Error("You have not selected a .GSC file! Please drag a GSC file over the top of Hologram.exe");

                    return;
                }

                if (args[0] == "extract")
                {
                    Logger.Log(new LogSeg("Export mode enabled!", ConsoleColor.DarkYellow));
                    gscFile = GSC.Parse(args[1]);
                    string newLocation = args[1];
                    if (Path.IsPathFullyQualified(args[1]))
                    {
                        string folder = Path.Combine(Path.GetDirectoryName(newLocation), Path.GetFileNameWithoutExtension(args[1]));
                        Directory.CreateDirectory(folder);
                        newLocation = Path.Combine(folder, Path.GetFileName(args[1]));
                    }
                    DAE.Create(Path.ChangeExtension(newLocation, "dae"), gscFile.entities);
                }
                else
                {
                    gscFile = GSC.Parse(args[0]);
                }
#endif
                window.AddEntities(gscFile.entities);

                window.Run();
            }
        }
    }
}