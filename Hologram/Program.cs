using Hologram.Rendering;
using Hologram.Objects;
using ModLib;

using OpenTK.Mathematics;
using Hologram.FileTypes.DNO;
using Hologram.FileTypes.GSC;
using Hologram.FileTypes;
using Hologram.FileTypes.DDS;
using System.Text;

namespace Hologram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (MainWindow window = new MainWindow())
            {
                //GSC test = GSC.Parse(args[0]);
                //GSC test = GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\TARDIS\TARDIS8\TARDIS8_NXG.GSC");
                GSC test = GSC.Parse(@"A:\Dimensions\EXTRACT\LEVELS\STORY\9GHOSTBUSTERS\9GHOSTBUSTERSA\TECH\9GHOSTBUSTERSA_TECH_NXG.GSC");

                //StringBuilder mat = new StringBuilder();
                //StringBuilder scene = new StringBuilder();
                //int totalCount = 1;
                //int matCount = 0;
                //scene.AppendLine($"mtllib testmtl.mtl");
                //for (int entId = 0; entId < test.entities.Length; entId++)
                //{
                //    Entity ent = test.entities[entId];

                //    mat.AppendLine($"newmtl mat{matCount}");
                //    mat.AppendLine("Ka 1.000000 1.000000 1.000000");
                //    mat.AppendLine($"Kd {ent.Mesh.Color.R} {ent.Mesh.Color.G} {ent.Mesh.Color.B}");
                //    mat.AppendLine("Ks 1.000000 1.000000 1.000000");
                //    mat.AppendLine("Ns 0.0");
                //    matCount++;

                //    scene.AppendLine($"usemtl mat{matCount}");
                //    scene.AppendLine($"o part{entId}");

                //    foreach (var vertex in ent.Mesh.Vertices)
                //    {
                //        Vector4 posRaw = new Vector4(vertex.Position, 1);
                //        Vector3 globalPos = new Vector3(Vector4.Dot(ent.Transformation.Column0, posRaw), Vector4.Dot(ent.Transformation.Column1, posRaw), Vector4.Dot(ent.Transformation.Column2, posRaw));
                //        scene.AppendLine($"v {globalPos.X} {globalPos.Y} {globalPos.Z}");
                //    }

                //    for (int indice = 0; indice < ent.Mesh.IndicesCount; indice += 3)
                //    {
                //        scene.AppendLine($"f {ent.Mesh.Indices[indice] + totalCount} {ent.Mesh.Indices[indice + 1] + totalCount} {ent.Mesh.Indices[indice + 2] + totalCount}");
                //    }

                //    totalCount += ent.Mesh.VertexCount;
                //}

                //File.WriteAllText(@"A:\testscene.obj", scene.ToString());
                //File.WriteAllText(@"A:\testmtl.mtl", mat.ToString());

                //DDS.Load(@"A:\Dimensions\EXTRACT\LEVELS\STORY\1WIZARDOFOZ\1WIZARDOFOZA\1WIZARDOFOZA_NXG\LEGO_Zeus\LEGO_Zeus_Images_Nut\brick\nd_yellowbrickroad_nostud_diff.nut.dds");
                //MeshX mesh = test.ConvertPart(test.parts[0]);
                //window.AddMesh(mesh, true);
                window.AddEntities(test.entities);

                window.Run();
            }
        }
    }
}