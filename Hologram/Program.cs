using Hologram.Rendering;
using Hologram.FileTypes.GSC;
using Hologram.Objects;
using Hologram.FileTypes;
using ModLib;

using OpenTK.Mathematics;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                //window.AddMesh(OBJ.Parse(@"A:\icosphere.obj").PhysicsMesh, true);

                Mesh mesh = new Mesh(6, 4, FaceType.Triangles);

                mesh.Vertices[0] = new Vector3(-10, -10, 0);
                mesh.Vertices[1] = new Vector3(-10, 10, 0);
                mesh.Vertices[2] = new Vector3(0, -10, 0);
                mesh.Vertices[3] = new Vector3(0, 10, 0);
                mesh.Vertices[4] = new Vector3(10, -10, 0);
                mesh.Vertices[5] = new Vector3(10, 10, 5);

                mesh.Faces[0].vert1 = 0;
                mesh.Faces[0].vert2 = 1;
                mesh.Faces[0].vert3 = 2;

                mesh.Faces[1].vert1 = 1;
                mesh.Faces[1].vert2 = 2;
                mesh.Faces[1].vert3 = 3;

                mesh.Faces[2].vert1 = 2;
                mesh.Faces[2].vert2 = 3;
                mesh.Faces[2].vert3 = 4;

                mesh.Faces[3].vert1 = 4;
                mesh.Faces[3].vert2 = 5;
                mesh.Faces[3].vert3 = 3;

                window.AddMesh(mesh, true);

                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_TER.DNO", 0x1ca).PhysicsMesh, true);
                //Mesh mesh = DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZB\1WIZARDOFOZB_TER.DNO", 0x1ca).PhysicsMesh;
                //window.AddMesh(DNO.Parse(@"A:\Dimensions\EXTRACT\LEVELS\HUB\VORTON\VORTON_TERR.DNO", 0x149).PhysicsMesh, true);
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