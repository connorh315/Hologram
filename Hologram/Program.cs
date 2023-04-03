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
using NativeFileDialogSharp;
using Hologram.Resources;

using Hologram.FileTypes.GSCWrite;
using Hologram.Objects.Entities;
using Hologram.Engine.UI;

namespace Hologram;

internal class Program
{
    public static Texture test;
    static void Main(string[] args)
    {
        using (MainWindow window = new MainWindow())
        {
            test = DDS.Load(@"A:\Dimensions\EXTRACT\LEVELS\STORY\2THESIMPSONS\2THESIMPSONSD\2THESIMPSONSD_NXG_correct\LEGO_Zeus\LEGO_Zeus_Images_Nut\simpsons\nd_simpclouds_diff.nut.dds");
#if DEBUG
            //OBJ arrow = OBJ.Parse(@"A:\Dimensions\Resources\Arrow.obj");
            //HOB.Write(@"A:\Dimensions\Resources\Arrow.hob", arrow.Entity);
            //window.Entities.AddRange(gscFile.entities);
            //OBJ obj_white = OBJ.Parse(@"A:\Dimensions\scenetest\whiteside.obj");
            //OBJ.DefaultVertexColor = new Color4(40, 40, 40, 255);
            //OBJ obj_black = OBJ.Parse(@"A:\Dimensions\scenetest\blackside.obj");
            //Entity[] test = new Entity[] { obj_white.Entity, obj_black.Entity };
            //window.Entities.AddRange(test);
            //GSCWriter.Write(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC", test);
            //DAE.Create("C:/users/connor/Desktop/DAE/test.dae", gscFile.entities);
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
            //window.Entities.AddRange(gscFile.entities);

            window.Run();
        }
    }
}