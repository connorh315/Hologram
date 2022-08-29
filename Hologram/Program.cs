using Hologram.Rendering;
using Hologram.FileTypes.GSC;
using Hologram.Objects;
using Hologram.FileTypes;
using ModLib;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                //Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZACOPY.DNO", 0x1ca).PhysicsMesh;
                //Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZB\1WIZARDOFOZB_TER.DNO", 0x1ca).PhysicsMesh;
                window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\HUB\VORTON\VORTON_TERR.DNO", 0x149).PhysicsMesh, true);
                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_TER.DNO", 0x151).PhysicsMesh);
                //Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZFREEPLAY\1WIZARDOFOZFREEPLAY_TER.DNO", 0x2a).PhysicsMesh;
                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\7APERTURELABS\7APERTURELABSCHAMBER01\7APERTURELABSCHAMBER01_TER.DNO", 0x2ae).PhysicsMesh);
                //window.AddMesh(GSCFile.Read(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG.GSC"));

                //int completed = 0;
                //foreach (string meshable in Directory.EnumerateFiles(@"A:\Dimensions\EXTRACT\LEVELS\STORY", "*.DNO", SearchOption.AllDirectories))
                //{
                //    if (completed == 30) break;
                //    try
                //    {
                //        Mesh mesh = DNO.Parse(meshable, 2).PhysicsMesh;
                //        mesh.Name = Path.GetFileName(meshable);
                //        window.AddMesh(mesh);
                //        completed++;
                //    }
                //    catch (Exception)
                //    {
                //        Logger.Log(new LogSeg("Failed to parse DNO: ", ConsoleColor.Red), new LogSeg(meshable));
                //    }
                //}


                //Mesh mesh = GSCFile.Read(@"A:\Dimensions\game\LEVELS\BUILDER\TESTCANDY\TESTCANDYJL\TESTCANDYJL_NXG.GSC");

                window.Run();
            }
        }
    }
}