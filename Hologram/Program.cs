﻿using Hologram.Rendering;
using Hologram.FileTypes.GSC;
using Hologram.Objects;
using ModLib;

using OpenTK.Mathematics;
using Hologram.FileTypes.DNO;
using Hologram.FileTypes;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                #region TestDNO
                //int successful = 0;
                //int failed = 0;
                //foreach (string dnoFile in Directory.EnumerateFiles(@"A:\Dimensions\EXTRACT\LEVELS\STORY", "*.DNO", SearchOption.AllDirectories))
                //{
                //    Console.WriteLine(dnoFile);

                //    try
                //    {
                //        DNO dnoTest = DNO.Parse(dnoFile, 0x1);

                //        Logger.Log(new LogSeg(dnoFile, ConsoleColor.Green));
                //        successful++;
                //    }
                //    catch (Exception ex)
                //    {
                //        if (ex.Message == "Type == 9")
                //        {
                //            Logger.Log(new LogSeg(dnoFile, ConsoleColor.DarkRed));
                //        }
                //        else
                //        {
                //            Logger.Log(new LogSeg(dnoFile, ConsoleColor.DarkYellow));
                //        }
                //        failed++;
                //    }
                //}
                //Console.WriteLine("Successfully read: {0}", successful);
                //Console.WriteLine("Failed reading: {0}", failed);
                #endregion

                //Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZB\1WIZARDOFOZB_TER.DNO", 0x1ca).PhysicsMesh;
                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\HUB\VORTON\VORTON_TERR.DNO", 0x149).Meshes[0].GetPhysicsMesh(), true);
                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_TER.DNO", 0x151).Meshes[0].GetPhysicsMesh(), true);
                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZFREEPLAY\1WIZARDOFOZFREEPLAY_TER.DNO", 0x2a).Meshes[0].GetPhysicsMesh(), true);
                //window.AddMesh(.PhysicsMesh);
                ;
                //Mesh mesh = Hologram.FileTypes.GSCNew.GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC").ConvertToMesh();
                Mesh mesh = Hologram.FileTypes.GSCNew.GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\TECH\1WIZARDOFOZA_TECH_NXG.GSC").ConvertToMesh();
                //Mesh mesh = Hologram.FileTypes.GSCNew.GSC.Parse(@"A:\Dimensions\EXTRACT\CHARCACHE\EGONSPENGLER_R3\CHARS\SUPER_CHARACTER\HAIR\HAIR_GREASER_R3_NXG.GSC").ConvertToMesh();
                //Mesh mesh = Hologram.FileTypes.GSCNew.GSC.Parse(@"A:\Dimensions\game\LEVELS\BUILDER\TESTCANDY\TESTCANDYJL\TESTCANDYJL_NXG.GSC").ConvertToMesh();
                //window.AddMesh(GSCFile.Read(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC"), true);
                window.AddMesh(mesh, true);

                //window.AddMesh(Hologram.FileTypes.GSCNew.GSC.Parse(@"A:\Dimensions\EXTRACT\CHARCACHE\JOKER\CHARS\SUPER_CHARACTER\HAIR\HAIR_GREASER_R3_NXG.GSC").ConvertToMesh(), true);

                //OBJ obj = OBJ.Parse(@"A:\Toilet2.obj");
                //window.AddMesh(obj.PhysicsMesh, true);
                //obj.PhysicsMesh.Setup();
                //GSCWrite.Write(obj.PhysicsMesh, @"A:\Dimensions\EXTRACT\CHARCACHE\JOKER\CHARS\SUPER_CHARACTER\HAIR\HAIR_GREASER_R3_NXG.GSC");

                //int completed = 0;
                //foreach (string meshable in Directory.EnumerateFiles(@"A:\Dimensions\EXTRACT\LEVELS\STORY", "*.DNO", SearchOption.AllDirectories))
                //{
                //    DNO dno = DNO.Parse(meshable, 0x1);
                //    foreach (DMesh mesh in dno.Meshes)
                //    {
                //        window.AddMesh(mesh.GetPhysicsMesh(), true);
                //    }
                //}



                window.Run();
            }
        }
    }
}